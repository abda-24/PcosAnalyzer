using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Diagnosis
{
    public class ImagePredictionResponseDto
    {
        [JsonPropertyName("prediction")]
        public string? Prediction { get; set; }

        [JsonPropertyName("confidence")]
        [JsonConverter(typeof(ConfidenceDoubleConverter))]
        public double? Confidence { get; set; }

        [JsonPropertyName("gradcam_image")]
        public string? GradcamImage { get; set; }

        [JsonPropertyName("details")]
        public PredictionDetails? Details { get; set; }
    }

    public class ConfidenceDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDouble();

            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str)) return null;
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
