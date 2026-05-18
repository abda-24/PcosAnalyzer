using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controller;
using ServicesAbstractions;
using Shared.DTOs.Diagnosis;
using AiModels = Shared.DTOs.AI.PcosDiagnostic;
using System.Security.Claims;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DiagnosisController : BaseController
    {
        private readonly IDiagnosisService _diagnosisService;

        public DiagnosisController(IDiagnosisService diagnosisService)
        {
            _diagnosisService = diagnosisService;
        }

        private string? CurrentUserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("uid")
            ?? User.FindFirstValue("sub");

       

        [HttpPost("ai-diagnose")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AiDiagnose([FromForm] AiModels.AiDiagnoseRequestDto request)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (string.IsNullOrEmpty(request.ClinicalData) && request.UltrasoundImage == null)
                return BadRequest("Please provide clinical data, image, or both.");

            ClinicalDataRequestDto? clinicalData = null;
            if (!string.IsNullOrEmpty(request.ClinicalData))
            {
                try
                {
                    clinicalData = JsonSerializer.Deserialize<ClinicalDataRequestDto>(
                        request.ClinicalData,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch
                {
                    return BadRequest("Invalid clinical data JSON.");
                }
            }

            var result = await _diagnosisService.AiDiagnoseAsync(userId, clinicalData, request.UltrasoundImage);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var history = await _diagnosisService.GetUserHistoryAsync(userId);
            return Ok(history);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var dashboard = await _diagnosisService.GetDashboardDataAsync(userId);
            return Ok(dashboard);
        }
    }
}
