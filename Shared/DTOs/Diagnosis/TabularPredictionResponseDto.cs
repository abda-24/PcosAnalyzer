using System.Text.Json.Serialization;

namespace Shared.DTOs.Diagnosis
{
    public class TabularPredictionResponseDto
    {
        [JsonPropertyName("prediction")]
        public string prediction { get; set; }
    }
}