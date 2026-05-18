using Shared.DTOs.AI.PcosDiagnostic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.Diagnosis
{
    public class PcosExplainabilityDto
    {
        [JsonPropertyName("clinical_reasons")]
        public PcosClinicalReasonsDto ClinicalReasons { get; set; } = new();

        [JsonPropertyName("ultrasound_heatmap_base64")]
        public string? UltrasoundHeatmapBase64 { get; set; }
    }
}
