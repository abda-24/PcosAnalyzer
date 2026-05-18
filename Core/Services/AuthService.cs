using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServicesAbstractions;
using Shared.DTOs.AuthDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthMessageDto> RegisterAsync(RegisterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthMessageDto { Message = "Email is already registered!", IsAuthenticated = false };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthMessageDto { Message = "Username is already taken!", IsAuthenticated = false };

            var user = _mapper.Map<ApplicationUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthMessageDto { Message = errors, IsAuthenticated = false };
            }

            return new AuthMessageDto { IsAuthenticated = true, Message = "User registered successfully" };
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthResultDto { Message = "Invalid Email or Password!", IsAuthenticated = false };

            var jwtToken = await CreateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthResultDto
            {
                IsAuthenticated = true,
                Message = "Login successful",
                UserName = user.UserName,
                Email = user.Email,
                Token = jwtToken.Token,
                ExpiresOn = jwtToken.ExpiresOn,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return new AuthResultDto { Message = "Invalid token", IsAuthenticated = false };

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return new AuthResultDto { Message = "Inactive token", IsAuthenticated = false };

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newJwtToken = await CreateJwtTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthResultDto
            {
                IsAuthenticated = true,
                Message = "Token refreshed successfully",
                UserName = user.UserName,
                Email = user.Email,
                Token = newJwtToken.Token,
                ExpiresOn = newJwtToken.ExpiresOn,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }

        public async Task<UserProfileDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return null;

            return new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                TotalAnalyses = user.TotalAnalyses,
                LastDiagnosis = user.LastDiagnosis,
                MemberSince = user.MemberSince
            };
        }

        public async Task<AuthMessageDto> UpdateProfileAsync(string currentEmail, UpdateProfileDto model)
        {
            var user = await _userManager.FindByEmailAsync(currentEmail);
            if (user == null) return new AuthMessageDto { Message = "User not found!", IsAuthenticated = false };

            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthMessageDto { Message = errors, IsAuthenticated = false };
            }

            return new AuthMessageDto { IsAuthenticated = true, Message = "Profile updated successfully" };
        }

        public async Task<AuthMessageDto> ChangePasswordAsync(string email, ChangePasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new AuthMessageDto { Message = "User not found!", IsAuthenticated = false };

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthMessageDto { Message = errors, IsAuthenticated = false };
            }

            return new AuthMessageDto { IsAuthenticated = true, Message = "Password changed successfully" };
        }

        private async Task<(string Token, DateTime ExpiresOn)> CreateJwtTokenAsync(ApplicationUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = _configuration["JWT:Key"];
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var durationInDays = double.Parse(_configuration["JWT:DurationInDays"] ?? "1");

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddDays(durationInDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (
                new JwtSecurityTokenHandler().WriteToken(tokenDescriptor),
                tokenDescriptor.ValidTo
            );
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}
