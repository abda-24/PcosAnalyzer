using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Diagnosis
{
    public class DashboardDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalAnalyses { get; set; }
        public List<AnalysisHistoryDto> History { get; set; } = new(); // السطر ده اللي بيحل المشكلة
    }
}
