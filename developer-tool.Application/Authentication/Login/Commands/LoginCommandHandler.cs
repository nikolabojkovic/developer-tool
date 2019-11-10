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
using Core.Options;
using Microsoft.Extensions.Options;

namespace Application.Authentication.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenViewModel>
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IRepository<UserModel> _userRepo;

        public LoginCommandHandler(IOptions<JwtOptions> jwtOptions, IRepository<UserModel> userRepo)
         {
             _jwtOptions = jwtOptions.Value;
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwtOptions.Issuer,
            _jwtOptions.Issuer,
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
