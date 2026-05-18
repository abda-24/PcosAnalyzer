using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Data.Context;
using Services.Configuration;
using ServicesAbstractions;
using Shared.DTOs.Diagnosis;
using AiModels = Shared.DTOs.AI.PcosDiagnostic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Data.Services
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly SmartPcosDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PcosDiagnosticSettings _settings;

        private static readonly JsonSerializerOptions _debugOpts =
            new JsonSerializerOptions { WriteIndented = true };

        public DiagnosisService(
            SmartPcosDbContext context,
            HttpClient httpClient,
            UserManager<ApplicationUser> userManager,
            IOptions<PcosDiagnosticSettings> settings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _settings = settings.Value;
        }

        public async Task<object> ProcessFlexibleDiagnosisAsync(
            string userId, ClinicalDataRequestDto? clinicalData, IFormFile? imageFile)
        {
            if (clinicalData == null && imageFile == null)
                return new { status = "Error", message = "Please provide either Clinical Data, an Ultrasound Image, or both." };

            // Save clinical data to DB
            string? clinicalDataJson = null;
            if (clinicalData != null)
            {
                await SaveClinicalDataOnlyAsync(clinicalData, userId);
                var tempRecord = MapToEntity(clinicalData, userId);
                clinicalDataJson = JsonSerializer.Serialize(BuildFlatDictionary(tempRecord));
            }

            var baseUrl = (_settings.BaseUrl ?? "https://mohamedyahya72-pcos-diagnostic-engine.hf.space").TrimEnd('/');
            var endpoint = $"{baseUrl}/api/v1/diagnose";

            HttpResponseMessage response;
            Stream? imageStream = null;

            try
            {
                // Mirror the original sending strategy: JSON body for clinical-only, multipart otherwise
                if (imageFile == null && !string.IsNullOrEmpty(clinicalDataJson))
                {
                    var stringContent = new StringContent(clinicalDataJson, System.Text.Encoding.UTF8, "application/json");
                    response = await _httpClient.PostAsync(endpoint, stringContent);
                }
                else
                {
                    var content = new MultipartFormDataContent();
                    if (!string.IsNullOrEmpty(clinicalDataJson))
                    {
                        var clinicalPart = new StringContent(clinicalDataJson, System.Text.Encoding.UTF8, "application/json");
                        content.Add(clinicalPart, "clinical_data");
                    }
                    if (imageFile != null)
                    {
                        imageStream = imageFile.OpenReadStream();
                        var fileContent = new StreamContent(imageStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                        content.Add(fileContent, "ultrasound_image", imageFile.FileName);
                    }
                    response = await _httpClient.PostAsync(endpoint, content);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    return new { status = "Error", message = $"AI returned {(int)response.StatusCode}: {raw}" };
                }

                var rawJson = await response.Content.ReadAsStringAsync();

                Console.WriteLine("=== RAW AI RESPONSE (ProcessFlexible) ===");
                Console.WriteLine(rawJson);

                // Save ultrasound image record — extract only the fields we need, leave the rest untouched
                if (imageFile != null)
                {
                    using var tempDoc = JsonDocument.Parse(rawJson);
                    var root = tempDoc.RootElement;

                    string? prediction = root.TryGetProperty("diagnosis", out var d) ? d.GetString() : null;
                    double? confidence = null;
                    if (root.TryGetProperty("overall_risk_score", out var ors) && ors.ValueKind == JsonValueKind.Number)
                        confidence = ors.GetDouble() * 100;
                    string? heatmap = null;
                    if (root.TryGetProperty("explainability", out var exp)
                        && exp.TryGetProperty("ultrasound_heatmap_base64", out var h)
                        && h.ValueKind == JsonValueKind.String)
                        heatmap = h.GetString();

                    await _context.UltrasoundImages.AddAsync(new UltrasoundImage
                    {
                        UserId = userId,
                        ImagePath = imageFile.FileName,
                        AiPrediction = string.IsNullOrEmpty(prediction) ? "Unknown" : prediction,
                        Confidence = confidence,
                        HeatmapBase64 = heatmap,
                        UploadedAt = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                }

                // Return the raw AI JSON as-is — no DTO mapping, no value corruption
                using var resultDoc = JsonDocument.Parse(rawJson);
                return resultDoc.RootElement.Clone();
            }
            finally
            {
                imageStream?.Dispose();
            }
        }

        private async Task SaveClinicalDataOnlyAsync(ClinicalDataRequestDto model, string userId)
        {
            var clinicalData = MapToEntity(model, userId);
            await _context.ClinicalData.AddAsync(clinicalData);
            await _context.SaveChangesAsync();
        }

        public async Task<object> SaveClinicalDataAsync(ClinicalDataRequestDto model, string userId)
        {
            var clinicalData = MapToEntity(model, userId);
            await _context.ClinicalData.AddAsync(clinicalData);
            await _context.SaveChangesAsync();

            var payload = new { data = BuildFlatDictionary(clinicalData) };
            var response = await _httpClient.PostAsJsonAsync("https://mohamedyahya72-pcos-tabular-api.hf.space/predict", payload);
            return await response.Content.ReadFromJsonAsync<object>() ?? new { status = "Success" };
        }

        public async Task<DiagnosisResultDto?> AnalyzePcosAsync(string userId, IFormFile imageFile)
        {
            if (string.IsNullOrEmpty(userId) || imageFile == null) return null;

            var clinicalData = await _context.ClinicalData
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (clinicalData == null) return null;

            using var form = new MultipartFormDataContent();
            using var stream = imageFile.OpenReadStream();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            form.Add(streamContent, "file", imageFile.FileName);

            var imageResponse = await _httpClient.PostAsync("https://mohamedyahya72-pcos-api.hf.space/predict", form);
            var imageResult = await imageResponse.Content.ReadFromJsonAsync<ImagePredictionResponseDto>();

            var hasPcos = imageResult?.Prediction?.Contains("Infected") ?? false;

            var imageRecord = new UltrasoundImage
            {
                UserId = userId,
                ImagePath = imageFile.FileName,
                AiPrediction = imageResult?.Prediction,
                Confidence = imageResult?.Confidence ?? 0,
                HeatmapBase64 = imageResult?.GradcamImage,
                UploadedAt = DateTime.UtcNow
            };

            await _context.UltrasoundImages.AddAsync(imageRecord);
            await _context.SaveChangesAsync();

            return new DiagnosisResultDto
            {
                ImagePrediction = imageResult?.Prediction,
                Confidence = imageRecord.Confidence,
                FinalStatus = hasPcos ? "Infected" : "Healthy",
                AnalysisDate = DateTime.UtcNow,
                Heatmap = imageResult?.GradcamImage,
                Report = "Image analysis completed successfully."
            };
        }

        public async Task<AiModels.PcosDiagnosticResponseDto> AnalyzeMultimodalAsync(
            AiModels.PcosMultimodalRequestDto request)
        {
            var baseUrl = (_settings.BaseUrl ?? "https://mohamedyahya72-pcos-diagnostic-engine.hf.space").TrimEnd('/');
            var endpoint = $"{baseUrl}/api/v1/diagnose";

            HttpResponseMessage response;

            if (request.UltrasoundImage == null && !string.IsNullOrEmpty(request.ClinicalData))
            {
                var stringContent = new StringContent(
                    request.ClinicalData, System.Text.Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync(endpoint, stringContent);
            }
            else
            {
                using var content = new MultipartFormDataContent();

                if (!string.IsNullOrEmpty(request.ClinicalData))
                {
                    var clinicalContent = new StringContent(
                        request.ClinicalData, System.Text.Encoding.UTF8, "application/json");
                    content.Add(clinicalContent, "clinical_data");
                }

                if (request.UltrasoundImage != null)
                {
                    var imgStream = request.UltrasoundImage.OpenReadStream();
                    var fileContent = new StreamContent(imgStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.UltrasoundImage.ContentType);
                    content.Add(fileContent, "ultrasound_image", request.UltrasoundImage.FileName);
                }

                response = await _httpClient.PostAsync(endpoint, content);
            }

            var rawJson = await response.Content.ReadAsStringAsync();

            Console.WriteLine("=== RAW AI RESPONSE (AnalyzeMultimodal) ===");
            Console.WriteLine(rawJson);

            var result = JsonSerializer.Deserialize<AiModels.PcosDiagnosticResponseDto>(
                rawJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result != null)
            {
                result.Recommendation ??= result.Diagnosis.Contains("Healthy", StringComparison.OrdinalIgnoreCase)
                    ? "Maintain a healthy lifestyle."
                    : "Please consult with a specialist for further evaluation.";
            }

            Console.WriteLine("=== FINAL API RESPONSE (AnalyzeMultimodal) ===");
            Console.WriteLine(JsonSerializer.Serialize(result, _debugOpts));

            return result ?? new AiModels.PcosDiagnosticResponseDto { Status = "Error" };
        }

        // Pure pass-through: returns the raw AI JsonElement unchanged
        public async Task<object> AiDiagnoseAsync(
            string userId, ClinicalDataRequestDto? clinicalData, IFormFile? imageFile)
        {
            if (clinicalData == null && imageFile == null)
                return new { status = "Error", diagnosis = "Provide clinical data, image, or both." };

            var baseUrl = (_settings.BaseUrl ?? "https://mohamedyahya72-pcos-diagnostic-engine.hf.space").TrimEnd('/');
            var endpoint = $"{baseUrl}/api/v1/diagnose";

            using var content = new MultipartFormDataContent();

            if (clinicalData != null)
            {
                var entity = MapToEntity(clinicalData, userId);
                var clinicalJson = JsonSerializer.Serialize(BuildFlatDictionary(entity));
                var clinicalPart = new StringContent(clinicalJson, System.Text.Encoding.UTF8, "application/json");
                content.Add(clinicalPart, "clinical_data");
            }

            Stream? imageStream = null;
            if (imageFile != null)
            {
                imageStream = imageFile.OpenReadStream();
                var fileContent = new StreamContent(imageStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "ultrasound_image", imageFile.FileName);
            }

            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    return new { status = "Error", diagnosis = $"AI returned {(int)response.StatusCode}: {raw}" };
                }

                var rawJson = await response.Content.ReadAsStringAsync();

                Console.WriteLine("=== RAW AI RESPONSE (AiDiagnose) ===");
                Console.WriteLine(rawJson);

                // Save image record to DB without touching the JSON
                if (imageFile != null)
                {
                    using var tempDoc = JsonDocument.Parse(rawJson);
                    var root = tempDoc.RootElement;

                    string? prediction = root.TryGetProperty("diagnosis", out var d) ? d.GetString() : null;
                    double? confidence = null;
                    if (root.TryGetProperty("overall_risk_score", out var ors) && ors.ValueKind == JsonValueKind.Number)
                        confidence = ors.GetDouble() * 100;
                    string? heatmap = null;
                    if (root.TryGetProperty("explainability", out var exp)
                        && exp.TryGetProperty("ultrasound_heatmap_base64", out var h)
                        && h.ValueKind == JsonValueKind.String)
                        heatmap = h.GetString();

                    await _context.UltrasoundImages.AddAsync(new UltrasoundImage
                    {
                        UserId = userId,
                        ImagePath = imageFile.FileName,
                        AiPrediction = string.IsNullOrEmpty(prediction) ? "Unknown" : prediction,
                        Confidence = confidence,
                        HeatmapBase64 = heatmap,
                        UploadedAt = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                }

                // Clone so the element is independent of the JsonDocument lifetime
                using var resultDoc = JsonDocument.Parse(rawJson);
                var aiResponse = resultDoc.RootElement.Clone();

                Console.WriteLine("=== FINAL API RESPONSE (AiDiagnose) ===");
                Console.WriteLine(JsonSerializer.Serialize(aiResponse, _debugOpts));

                return aiResponse;
            }
            finally
            {
                imageStream?.Dispose();
            }
        }

        public async Task<List<AnalysisHistoryDto>> GetUserHistoryAsync(string userId)
        {
            return await _context.UltrasoundImages
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.UploadedAt)
                .Select(x => new AnalysisHistoryDto
                {
                    AnalysisId = x.Id,
                    Date = x.UploadedAt,
                    ImagePrediction = x.AiPrediction,
                    Confidence = x.Confidence,
                    FinalStatus = x.AiPrediction ?? "Unknown"
                })
                .ToListAsync();
        }

        public async Task<DashboardDto> GetDashboardDataAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var historyList = await GetUserHistoryAsync(userId);

            return new DashboardDto
            {
                UserName = user?.UserName ?? "Guest",
                Email = user?.Email ?? "",
                TotalAnalyses = historyList.Count,
                History = historyList
            };
        }

        private ClinicalData MapToEntity(ClinicalDataRequestDto model, string userId)
        {
            return new ClinicalData
            {
                UserId = userId,
                Age = model.Age,
                Weight = model.Weight,
                Height = model.Height,
                BMI = model.BMI,
                BloodGroup = model.BloodGroup,
                PulseRate = model.PulseRate,
                RR = model.RR,
                Hb = model.Hb,
                Cycle = model.Cycle,
                CycleLength = model.CycleLength,
                MarriageStatus = model.MarriageStatus,
                Pregnant = model.Pregnant,
                NoOfAbortions = model.NoOfAbortions,
                BetaHCG_I = model.BetaHCG_I,
                BetaHCG_II = model.BetaHCG_II,
                FSH = model.FSH,
                LH = model.LH,
                FSH_LH = model.FSH_LH,
                Hip = model.Hip,
                Waist = model.Waist,
                WaistHipRatio = model.WaistHipRatio,
                TSH = model.TSH,
                AMH = model.AMH,
                PRL = model.PRL,
                VitD3 = model.VitD3,
                PRG = model.PRG,
                RBS = model.RBS,
                WeightGain = model.WeightGain,
                HairGrowth = model.HairGrowth,
                SkinDarkening = model.SkinDarkening,
                HairLoss = model.HairLoss,
                Pimples = model.Pimples,
                FastFood = model.FastFood,
                RegExercise = model.RegExercise,
                BP_Systolic = model.BP_Systolic,
                BP_Diastolic = model.BP_Diastolic,
                FollicleNoL = model.FollicleNoL,
                FollicleNoR = model.FollicleNoR,
                AvgFSizeL = model.AvgFSizeL,
                AvgFSizeR = model.AvgFSizeR,
                Endometrium = model.Endometrium,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static Dictionary<string, object> BuildFlatDictionary(ClinicalData x)
        {
            return new Dictionary<string, object>
            {
                { "Age (yrs)", x.Age },
                { "Weight (Kg)", x.Weight },
                { "Height(Cm)", x.Height },
                { "BMI", x.BMI },
                { "Blood Group", x.BloodGroup },
                { "Pulse rate(bpm)", x.PulseRate },
                { "RR (breaths/min)", x.RR },
                { "Hb(g/dl)", x.Hb },
                { "Cycle(R/I)", x.Cycle },
                { "Cycle length(days)", x.CycleLength },
                { "Marraige Status (Yrs)", x.MarriageStatus },
                { "Pregnant(Y/N)", x.Pregnant ? 1 : 0 },
                { "No. of aborptions", x.NoOfAbortions },
                { "I   beta-HCG(mIU/mL)", x.BetaHCG_I },
                { "II    beta-HCG(mIU/mL)", x.BetaHCG_II },
                { "FSH(mIU/mL)", x.FSH },
                { "LH(mIU/mL)", x.LH },
                { "FSH/LH", x.FSH_LH },
                { "Hip(inch)", x.Hip },
                { "Waist(inch)", x.Waist },
                { "Waist:Hip Ratio", x.WaistHipRatio },
                { "TSH (mIU/L)", x.TSH },
                { "AMH(ng/mL)", x.AMH },
                { "PRL(ng/mL)", x.PRL },
                { "Vit D3 (ng/mL)", x.VitD3 },
                { "PRG(ng/mL)", x.PRG },
                { "RBS(mg/dl)", x.RBS },
                { "Weight gain(Y/N)", x.WeightGain ? 1 : 0 },
                { "hair growth(Y/N)", x.HairGrowth ? 1 : 0 },
                { "Skin darkening (Y/N)", x.SkinDarkening ? 1 : 0 },
                { "Hair loss(Y/N)", x.HairLoss ? 1 : 0 },
                { "Pimples(Y/N)", x.Pimples ? 1 : 0 },
                { "Fast food (Y/N)", x.FastFood ? 1 : 0 },
                { "Reg.Exercise(Y/N)", x.RegExercise ? 1 : 0 },
                { "BP _Systolic (mmHg)", x.BP_Systolic },
                { "BP _Diastolic (mmHg)", x.BP_Diastolic },
                { "Follicle No. (L)", x.FollicleNoL },
                { "Follicle No. (R)", x.FollicleNoR },
                { "Avg. F size (L) (mm)", x.AvgFSizeL },
                { "Avg. F size (R) (mm)", x.AvgFSizeR },
                { "Endometrium (mm)", x.Endometrium }
            };
        }
    }
}
