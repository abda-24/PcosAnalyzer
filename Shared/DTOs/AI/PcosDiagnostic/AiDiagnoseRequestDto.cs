using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.AI.PcosDiagnostic
{
    public class AiDiagnoseRequestDto
    {
        public string? ClinicalData { get; set; }
        public IFormFile? UltrasoundImage { get; set; }
    }
}
