using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServicesAbstractions;
using Shared.DTOs.Contact;
using System.Net;

namespace Services
{
    public class ContactService : IContactService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IConfiguration configuration, ILogger<ContactService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendContactMessageAsync(ContactUsDto model, string? userId = null)
        {
            try
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.Name) ||
                    string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Message))
                {
                    _logger.LogWarning("Invalid contact form submission");
                    return false;
                }

                var emailSettings = _configuration.GetSection("MailSettings");

                var fromEmail = emailSettings["Email"];
                var password = emailSettings["Password"];
                var host = emailSettings["Host"];
                int port = int.TryParse(emailSettings["Port"], out var p) ? p : 587;

                if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(host))
                {
                    _logger.LogError("Email configuration is missing");
                    return false;
                }

                var safeName = WebUtility.HtmlEncode(model.Name);
                var safeEmail = WebUtility.HtmlEncode(model.Email);
                var safeMessage = WebUtility.HtmlEncode(model.Message);
                var safeUserId = WebUtility.HtmlEncode(userId ?? "Guest User");

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("SmartFlow Support", fromEmail));
                email.To.Add(MailboxAddress.Parse(fromEmail));
                email.ReplyTo.Add(MailboxAddress.Parse(model.Email));
                email.Subject = $"New Contact Message from {safeName}";

                var builder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif;'>
                            <h3 style='color: #2e6ca4;'>New Inquiry from SmartFlow App</h3>
                            <p><b>Name:</b> {safeName}</p>
                            <p><b>Email:</b> {safeEmail}</p>
                            <p><b>Message:</b><br/>{safeMessage}</p>
                            <hr/>
                            <p style='font-size: 0.8em; color: gray;'>User ID: {safeUserId}</p>
                        </div>"
                };

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(fromEmail, password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email inquiry from {Email}", model?.Email);
                return false;
            }
        }
    }
}
