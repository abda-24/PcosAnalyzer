using System.Text.Json.Serialization;

public class ImagePredictionResponse
{
    [JsonPropertyName("prediction")]
    public string Prediction { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public string Confidence { get; set; } = string.Empty;

    [JsonPropertyName("gradcam_image")]
    public string GradcamImage { get; set; } = string.Empty;
}
