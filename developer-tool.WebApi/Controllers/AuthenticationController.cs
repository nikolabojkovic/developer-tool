using System.Threading.Tasks;
using Application.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers 
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register() 
        {
            return Ok(await _mediator.Send(new RegisterCommand()));
        }
    }
}