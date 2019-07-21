using System.Threading;
using System.Threading.Tasks;
using Application.Authentication.CommandHandlers;
using Application.Authentication.Commands;
using AutoMapper;
using Core.Interfaces;
using Domain.Models;
using Domain.PersistenceModels;
using Moq;
using Xunit;

namespace TestUnit.UsesrTests.CommandHandlers 
{
    public class RegisterUserCoomandTest
    {
        [Fact]
        public async Task Register_NewUser_ShouldPersistNewUser()
        {
            // Arrange
            Mock<IRepository<UserModel>> userRepo = new Mock<IRepository<UserModel>>();
            userRepo.Setup(repo => repo.AddAsync(It.IsAny<UserModel>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new UserModel()));
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<UserModel>(It.IsAny<User>()))
                          .Returns(new UserModel());
            var handler = new RegisterCommandHandler(userRepo.Object, mapperMock.Object);

            // Act
            await handler.Handle(new RegisterCommand {
                Username = "testUsername",
                Password = "test123",
                FirstName = "Ftest",
                LastName = "Ltest"
            }, It.IsAny<CancellationToken>());

            // Assert
            userRepo.Verify(x => x.AddAsync(It.IsAny<UserModel>(), It.IsAny<CancellationToken>()), Times.Once);
            mapperMock.Verify(x => x.Map<UserModel>(It.IsAny<User>()), Times.Once);
        }
    }
}