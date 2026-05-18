using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.DTOs.AI.PcosDiagnostic
{
    public class PcosClinicalDataDto
    {
        public double? Age { get; set; }
        public double? BMI { get; set; }
        public double? FSH { get; set; }
        public double? LH { get; set; }
    }

    public class PcosFactorDto
    {
        [JsonPropertyName("feature")]
        public string? Feature { get; set; }

        // JsonElement preserves whatever the AI sent (number, string, null) verbatim
        [JsonPropertyName("value")]
        public JsonElement? Value { get; set; }

        [JsonPropertyName("impact_score")]
        public JsonElement? ImpactScore { get; set; }

        [JsonPropertyName("shap_value")]
        public JsonElement? ShapValue { get; set; }
    }

    public class PcosClinicalReasonsDto
    {
        [JsonPropertyName("risk_increasing_factors")]
        public List<PcosFactorDto> RiskIncreasingFactors { get; set; } = new();

        [JsonPropertyName("protective_factors")]
        public List<PcosFactorDto> ProtectiveFactors { get; set; } = new();
    }

    public class PcosExplainabilityDto
    {
        [JsonPropertyName("clinical_reasons")]
        public PcosClinicalReasonsDto? ClinicalReasons { get; set; }

        [JsonPropertyName("ultrasound_heatmap_base64")]
        public string? UltrasoundHeatmapBase64 { get; set; }
    }

    public class PcosBreakdownDto
    {
        // JsonElement? preserves "N/A" as string and 0.126 as number without conversion
        [JsonPropertyName("clinical_risk")]
        public JsonElement? ClinicalRisk { get; set; }

        [JsonPropertyName("ultrasound_risk")]
        public JsonElement? UltrasoundRisk { get; set; }
    }

    public class PcosDiagnosticResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("diagnosis")]
        public string Diagnosis { get; set; } = string.Empty;

        [JsonPropertyName("overall_risk_score")]
        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double? OverallRiskScore { get; set; }

        [JsonPropertyName("diagnostic_method")]
        public string DiagnosticMethod { get; set; } = string.Empty;

        [JsonPropertyName("breakdown")]
        public PcosBreakdownDto? Breakdown { get; set; }

        [JsonPropertyName("explainability")]
        public PcosExplainabilityDto? Explainability { get; set; }

        [JsonIgnore]
        public string? Heatmap => Explainability?.UltrasoundHeatmapBase64;

        [JsonIgnore]
        public string? Recommendation { get; set; }
    }

    public class FlexibleDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDouble();

            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str) || str.Equals("N/A", StringComparison.OrdinalIgnoreCase))
                    return null;
                str = str.Replace("%", "").Trim();
                if (double.TryParse(str, System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out var d))
                    return d;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
