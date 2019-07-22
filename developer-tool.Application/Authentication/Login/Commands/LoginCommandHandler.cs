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
using Core.Interfaces;
using Domain.PersistenceModels;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenViewModel>
    {
        private readonly IConfiguration _config;
        private readonly IRepository<UserModel> _userRepo;

        public LoginCommandHandler(IConfiguration config, IRepository<UserModel> userRepo)
         {
             _config = config;
             _userRepo = userRepo;
         }

        public async Task<TokenViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var expiresInMinutes = 30;

            var user = await _userRepo.Find(x => x.Username == request.Username && x.Password == request.Password)
                                      .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new BadRequestException("Wrong credentials.");

            // TODO: refactor, move claim and token creation in separate provider (Authntication provider)
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
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

        }
    }
}
