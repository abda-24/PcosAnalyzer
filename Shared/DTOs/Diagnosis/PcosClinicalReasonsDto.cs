using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.Diagnosis
{
    public class PcosClinicalReasonsDto
    {
        [JsonPropertyName("risk_increasing_factors")]
        public List<PcosFactorDto> RiskIncreasingFactors { get; set; } = new();

        [JsonPropertyName("protective_factors")]
        public List<PcosFactorDto> ProtectiveFactors { get; set; } = new();
    }
}
