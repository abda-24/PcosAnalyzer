using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Contact;
using System.Security.Claims;

namespace Presentation.Controller.Account
{
    
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ContactUsDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _contactService.SendContactMessageAsync(model, userId);

            if (result)
                return Ok(new { Message = "Your message has been sent successfully!" });

            return BadRequest(new { Message = "Failed to send the message. Please try again later." });
        }
    }
}