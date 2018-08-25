using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Email.Services;
using WebApi.ViewModels;

namespace WebApi.Controllers {

    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;
        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public void get() {

        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailViewModel contact)
        {
            await _emailService.SendEmail(contact.Name, contact.Phone, contact.Email, contact.Subject, contact.Message);
            return Ok();
        }
    }
}