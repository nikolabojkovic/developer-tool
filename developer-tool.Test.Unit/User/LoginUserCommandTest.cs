using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Authentication.CommandHandlers;
using Application.Authentication.Commands;
using Core.Exceptions;
using Core.Interfaces;
using Core.Options;
using Domain.PersistenceModels;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace TestUnit.UsesrTests.CommandHandlers 
{
    public class LoginUserCoomandTest
    {
        [Fact]
        public async Task Login_User_ShouldReturnToken()
        {
            // Arrange
            var expectedUsers = new UserModel[] { new UserModel { Username =  "admin", Password = "admin123" } };
            var mock = expectedUsers.AsQueryable().BuildMock();
            Mock<IRepository<UserModel>> userRepo = new Mock<IRepository<UserModel>>();
            userRepo.Setup(repo => repo.FindAll()).Returns(mock.Object);
            userRepo.Setup(repo => repo.Find(It.IsAny<Expression<Func<UserModel, bool>>>()))
                    .Returns(mock.Object);
            Mock<IOptions<JwtOptions>> config = new Mock<IOptions<JwtOptions>>();
            config.Setup(x => x.Value).Returns(new JwtOptions {
                Key = "testKey12312_woeifo23423",
                Issuer = "http://localhost:5000"
            });

            var handler = new LoginCommandHandler(config.Object, userRepo.Object);

            // Act
            await handler.Handle(new LoginCommand {
                Username = "admin",
                Password = "admin123"
            }, It.IsAny<CancellationToken>());

            // Assert
            userRepo.Verify(x => x.Find(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once);
        }

        [Theory]
        [InlineData("Wrong credentials.")]
        public async Task Login_User_ShouldReturnBadRequestException(string exceptionMessage)
        {
            // Arrange
            var expectedUsers = new UserModel[] { new UserModel { Username =  "admin", Password = "admin123" } };
            var mock = expectedUsers.AsQueryable().BuildMock();
            Mock<IRepository<UserModel>> userRepo = new Mock<IRepository<UserModel>>();
            userRepo.Setup(repo => repo.FindAll()).Returns(mock.Object);
            userRepo.Setup(repo => repo.Find(It.IsAny<Expression<Func<UserModel, bool>>>()))
                    .Returns(new UserModel[] { }.AsQueryable().BuildMock().Object);
            Mock<IOptions<JwtOptions>> config = new Mock<IOptions<JwtOptions>>();
            config.Setup(x => x.Value).Returns(new JwtOptions {
                Key = "testKey12312_woeifo23423",
                Issuer = "http://localhost:5000"
            });
 
            var handler = new LoginCommandHandler(config.Object, userRepo.Object);

            // Act
             var ex = await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new LoginCommand {
                Username = "admin",
                Password = "admin12345"
            }, It.IsAny<CancellationToken>()));
 
            // Assert
            ex.Message.Should().Be(string.Format(exceptionMessage, "testUsername"));
        }
    }
}