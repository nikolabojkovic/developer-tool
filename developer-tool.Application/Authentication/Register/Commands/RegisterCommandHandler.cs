using System;
using MediatR;
using Application.Authentication.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.CommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Unit>
    {
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // throw new NotFoundException(nameof(Todo), "123");
            await Task.Run(() => Console.WriteLine("Command executed"));

            return Unit.Value;
        }
    }
}
