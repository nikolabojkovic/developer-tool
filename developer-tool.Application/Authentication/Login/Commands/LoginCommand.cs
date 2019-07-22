using System;
using MediatR;

namespace Application.Authentication.Commands
{
    public class LoginCommand : IRequest<TokenViewModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
