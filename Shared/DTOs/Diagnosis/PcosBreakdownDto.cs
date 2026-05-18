using System.Text.Json.Serialization;

public class PcosBreakdownDto
{
    // التعديل هنا: خليناها object?
    [JsonPropertyName("clinical_risk")]
    public object? ClinicalRisk { get; set; }

    [JsonPropertyName("ultrasound_risk")]
    public object? UltrasoundRisk { get; set; } // المشكلة كلها كانت في السطر ده!
}