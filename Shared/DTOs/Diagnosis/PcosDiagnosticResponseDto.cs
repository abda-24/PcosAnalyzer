using Shared.DTOs.AI.PcosDiagnostic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.Diagnosis
{
    public class PcosDiagnosticResponseDto
    {
        [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
        [JsonPropertyName("diagnosis")] public string Diagnosis { get; set; } = string.Empty;
        [JsonPropertyName("overall_risk_score")] public double OverallRiskScore { get; set; }
        [JsonPropertyName("diagnostic_method")] public string DiagnosticMethod { get; set; } = string.Empty;
        [JsonPropertyName("breakdown")] public PcosBreakdownDto Breakdown { get; set; } = new();
        [JsonPropertyName("explainability")] public PcosExplainabilityDto Explainability { get; set; } = new();

        [JsonIgnore] public string? Heatmap => Explainability?.UltrasoundHeatmapBase64;
        [JsonIgnore] public string? Recommendation { get; set; }
    }
}
