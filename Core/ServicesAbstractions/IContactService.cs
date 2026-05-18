using Shared.DTOs.Contact;

namespace ServicesAbstractions
{
    public interface IContactService
    {
        Task<bool> SendContactMessageAsync(ContactUsDto model, string? userId = null);
    }
}
