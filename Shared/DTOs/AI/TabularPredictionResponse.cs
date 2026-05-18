using System.Text.Json.Serialization;

public class TabularPredictionResponse
{
    [JsonPropertyName("prediction")]
    public string Prediction { get; set; } = string.Empty;
}
