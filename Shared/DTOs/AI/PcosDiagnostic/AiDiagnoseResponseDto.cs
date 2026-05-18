using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.DTOs.AI.PcosDiagnostic
{
    public class AiDiagnoseResponseDto
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("diagnosis")]
        public string? Diagnosis { get; set; }

        [JsonPropertyName("overall_risk_score")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double? OverallRiskScore { get; set; }

        [JsonPropertyName("diagnostic_method")]
        public string? DiagnosticMethod { get; set; }

        [JsonPropertyName("breakdown")]
        public AiBreakdownDto? Breakdown { get; set; }

        [JsonPropertyName("explainability")]
        public AiExplainabilityDto? Explainability { get; set; }
    }

    public class AiBreakdownDto
    {
        // JsonElement? round-trips "N/A", numbers, and null without loss
        [JsonPropertyName("clinical_risk")]
        public JsonElement? ClinicalRisk { get; set; }

        [JsonPropertyName("ultrasound_risk")]
        public JsonElement? UltrasoundRisk { get; set; }
    }

    public class AiExplainabilityDto
    {
        [JsonPropertyName("clinical_reasons")]
        public AiClinicalReasonsDto? ClinicalReasons { get; set; }

        [JsonPropertyName("ultrasound_heatmap_base64")]
        public string? UltrasoundHeatmapBase64 { get; set; }
    }

    public class AiClinicalReasonsDto
    {
        [JsonPropertyName("risk_increasing_factors")]
        public List<AiFactorDto> RiskIncreasingFactors { get; set; } = new();

        [JsonPropertyName("protective_factors")]
        public List<AiFactorDto> ProtectiveFactors { get; set; } = new();
    }

    public class AiFactorDto
    {
        [JsonPropertyName("feature")]
        public string? Feature { get; set; }

        [JsonPropertyName("value")]
        public JsonElement? Value { get; set; }

        [JsonPropertyName("impact_score")]
        public JsonElement? ImpactScore { get; set; }

        [JsonPropertyName("shap_value")]
        public JsonElement? ShapValue { get; set; }
    }
}
