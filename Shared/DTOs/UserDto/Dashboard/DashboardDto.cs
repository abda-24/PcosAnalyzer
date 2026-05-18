using Shared.DTOs.Diagnosis;
using System.Collections.Generic;

namespace Shared.DTOs.UserDto.Dashboard
{
    public class DashboardUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalAnalyses { get; set; }
        public List<AnalysisHistoryDto> History { get; set; } = new();
    }
}
