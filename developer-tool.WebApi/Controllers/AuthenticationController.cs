using System.Threading.Tasks;
using Application.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.InputModels;

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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel loginModel) 
        {
            return Ok(await _mediator.Send(new LoginCommand {
                Username = loginModel.Username,
                Password = loginModel.Password
            }));
        }
    }
}