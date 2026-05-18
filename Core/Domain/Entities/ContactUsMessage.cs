namespace Domain.Entities
{
    public class ContactUsMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string SentAt { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
