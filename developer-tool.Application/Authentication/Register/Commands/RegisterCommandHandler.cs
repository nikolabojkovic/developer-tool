using MediatR;
using Application.Authentication.Commands;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using Core.Interfaces;
using Domain.PersistenceModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

namespace Application.Authentication.CommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Unit>
    {
        private readonly IRepository<UserModel> _userRepo;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IRepository<UserModel> userRepo, IMapper mapper) 
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {            
            if (await _userRepo.FindAll().AnyAsync(x => x.Username == request.Username, cancellationToken))
                throw new BadRequestException($"Username: {request.Username} is not available, plese choose other username.");

            var user = User.Create(request.Username, request.Password, request.FirstName, request.LastName);
            await _userRepo.AddAsync(_mapper.Map<UserModel>(user), cancellationToken);

            return Unit.Value;
        }
    }
}
