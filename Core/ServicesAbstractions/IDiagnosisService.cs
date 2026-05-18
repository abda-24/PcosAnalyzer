using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DTOs.Diagnosis;
using AiModels = Shared.DTOs.AI.PcosDiagnostic;

namespace ServicesAbstractions
{
    public interface IDiagnosisService
    {
        Task<object> ProcessFlexibleDiagnosisAsync(string userId, ClinicalDataRequestDto? clinicalData, IFormFile? imageFile);

        Task<object> SaveClinicalDataAsync(ClinicalDataRequestDto model, string userId);

        Task<DiagnosisResultDto?> AnalyzePcosAsync(string userId, IFormFile imageFile);

        Task<List<AnalysisHistoryDto>> GetUserHistoryAsync(string userId);

        Task<AiModels.PcosDiagnosticResponseDto> AnalyzeMultimodalAsync(AiModels.PcosMultimodalRequestDto request);

        // Returns object (JsonElement) — pure pass-through of the raw AI response
        Task<object> AiDiagnoseAsync(string userId, ClinicalDataRequestDto? clinicalData, IFormFile? imageFile);

        Task<DashboardDto> GetDashboardDataAsync(string userId);
    }
}
