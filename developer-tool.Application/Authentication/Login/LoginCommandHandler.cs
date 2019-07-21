using System;
using MediatR;
using Application.Authentication.Commands;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Authentication.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenViewModel>
    {
        private readonly IConfiguration _config;

        public LoginCommandHandler(IConfiguration config)
         {
             _config = config;
         }

        public async Task<TokenViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => 
            {
                var expiresInMinutes = 30;

                // check if credentials are correct

                // TODO: refactor, move claim and token creation in separate provider (Authntication provider)
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(expiresInMinutes),
                    signingCredentials: creds);

                    return new TokenViewModel { 
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        ExpiresIn = expiresInMinutes
                    };
            });

        }
    }
}
