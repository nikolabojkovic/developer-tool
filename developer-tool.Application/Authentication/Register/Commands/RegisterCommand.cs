using System;
using MediatR;

namespace Application.Authentication.Commands
{
    public class RegisterCommand : IRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
