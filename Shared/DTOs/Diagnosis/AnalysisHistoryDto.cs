using System;

namespace Shared.DTOs.Diagnosis
{
    public class AnalysisHistoryDto
    {
        public int AnalysisId { get; set; }
        public DateTime Date { get; set; }
        public string? ImagePrediction { get; set; }
        public string? FinalStatus { get; set; }
        public double? Confidence { get; set; }
    }
}
