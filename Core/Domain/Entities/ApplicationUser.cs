using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TotalAnalyses { get; set; }
        public string? LastDiagnosis { get; set; }
        public DateTime? MemberSince { get; set; }
        public virtual ICollection<ClinicalData> ClinicalDataRecords { get; set; }
        public virtual ICollection<HormoneLabResult> HormoneLabResults { get; set; }
        public virtual ICollection<UltrasoundImage> UltrasoundImages { get; set; }
        public virtual ICollection<ContactUsMessage> ContactUsMessages { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
