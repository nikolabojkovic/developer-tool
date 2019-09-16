using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core.Interfaces;
using WebApi.InputModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers {

    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;
        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send/email")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailInputModel contact)
        {
            await _emailService.SendEmail(contact.Name, contact.Phone, contact.Email, contact.Subject, contact.Message);
            return Ok();
        }
    }
}