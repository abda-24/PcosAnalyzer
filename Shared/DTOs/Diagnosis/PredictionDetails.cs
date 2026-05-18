using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.Diagnosis
{
    public class PredictionDetails
    {
        [JsonPropertyName("effnet_prob")] public string? effnet_prob { get; set; }
        [JsonPropertyName("densenet_prob")] public string? densenet_prob { get; set; }
    }
}
