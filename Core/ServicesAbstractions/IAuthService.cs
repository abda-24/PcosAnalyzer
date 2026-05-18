using Shared.DTOs.AuthDto;

namespace ServicesAbstractions
{
    public interface IAuthService
    {
        Task<AuthMessageDto> RegisterAsync(RegisterDto model);
        Task<AuthResultDto> LoginAsync(LoginDto model);
        Task<AuthResultDto> RefreshTokenAsync(string token);
        Task<UserProfileDto> GetCurrentUserAsync(string email);
        Task<AuthMessageDto> UpdateProfileAsync(string currentEmail, UpdateProfileDto model);
        Task<AuthMessageDto> ChangePasswordAsync(string email, ChangePasswordDto model);
    }
}
