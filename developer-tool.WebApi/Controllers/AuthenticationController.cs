using System.Threading.Tasks;
using Application.Authentication.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.InputModels;
using WebApi.Results;

namespace WebApi.Controllers 
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator, IMapper mapper) 
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel inputModel) 
        {
            await _mediator.Send(_mapper.Map<RegisterCommand>(inputModel));
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel loginModel) 
        {
            return SuccessObjectResult.Data(await _mediator.Send(new LoginCommand {
                Username = loginModel.Username,
                Password = loginModel.Password
            }));
        }
    }
}