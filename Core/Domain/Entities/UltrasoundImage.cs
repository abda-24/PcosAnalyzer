namespace Domain.Entities
{
    public class UltrasoundImage
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ImagePath { get; set; }

        public DateTime UploadedAt { get; set; }

        public string? AiPrediction { get; set; }

        public double? Confidence { get; set; }

        public string? HeatmapBase64 { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}