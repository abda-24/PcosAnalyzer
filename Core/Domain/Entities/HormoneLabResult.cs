namespace Domain.Entities
{
    public class HormoneLabResult
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public float Amh { get; set; }
        public float Lh { get; set; }
        public float Fsh { get; set; }
        public float Tsh { get; set; }

        // Additional fields for AI Accuracy
        public float Prl { get; set; }
        public float VitD3 { get; set; }
        public float Rbs { get; set; }
        public float Hb { get; set; }
        public float Prg { get; set; } // Progesterone
        public float BetaHCG_I { get; set; }
        public float BetaHCG_II { get; set; }

        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public virtual ApplicationUser User { get; set; }
    }
}