using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.AI.PcosDiagnostic
{
    /// <summary>
    /// DTO for multimodal PCOS diagnosis request.
    /// Supports clinical data (as JSON string) and/or ultrasound image.
    /// </summary>
    public class PcosMultimodalRequestDto
    {
        /// <summary>
        /// Optional JSON string containing clinical data.
        /// Example: {"Age": 26, "BMI": 31.5, "FSH": 5.2, "LH": 11.8}
        /// </summary>
        public string? ClinicalData { get; set; }

        /// <summary>
        /// Optional ultrasound image file.
        /// </summary>
        public IFormFile? UltrasoundImage { get; set; }
    }
}
