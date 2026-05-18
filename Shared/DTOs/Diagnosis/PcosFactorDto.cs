using System.Text.Json.Serialization;

public class PcosFactorDto
{
    [JsonPropertyName("feature")] public string? Feature { get; set; }

    [JsonPropertyName("value")] public object? Value { get; set; }
    [JsonPropertyName("impact_score")] public object? ImpactScore { get; set; }
    [JsonPropertyName("shap_value")] public object? ShapValue { get; set; }

    [JsonPropertyName("impact")] public string? Impact { get; set; }
}