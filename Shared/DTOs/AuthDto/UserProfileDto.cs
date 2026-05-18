namespace Shared.DTOs.AuthDto
{
    public class UserProfileDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TotalAnalyses { get; set; }
        public string LastDiagnosis { get; set; }
        public DateTime? MemberSince { get; set; }
    }
}