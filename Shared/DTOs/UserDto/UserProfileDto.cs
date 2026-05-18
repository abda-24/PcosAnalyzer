public class UserProfileDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public int TotalAnalyses { get; set; } // إجمالي عدد التحاليل
    public string LastDiagnosis { get; set; } // آخر حالة (Infected/Healthy)
    public DateTime? MemberSince { get; set; } // تاريخ التسجيل
}