using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Authentication.CommandHandlers;
using Application.Authentication.Commands;
using AutoMapper;
using Core.Interfaces;
using Domain.Models;
using Domain.PersistenceModels;
using Microsoft.Extensions.Configuration;
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
            var expectedTodos = new UserModel[] { new UserModel { Username =  "admin", Password = "admin123" } };
            Mock<IRepository<UserModel>> userRepo = new Mock<IRepository<UserModel>>();
            userRepo.Setup(repo => repo.Find(It.IsAny<Expression<Func<UserModel, bool>>>()))
                          .Returns(expectedTodos.AsQueryable());
            Mock<IConfiguration> config = new Mock<IConfiguration>();
            config.SetupGet(m => m[It.Is<string>(s => s == "Jwt:Key")]).Returns("testKey12312_woeifo23423");
            config.SetupGet(m => m[It.Is<string>(s => s == "Jwt:Issuer")]).Returns("http://localhost:5000");
 
            var handler = new LoginCommandHandler(config.Object, userRepo.Object);

            // Act
            await handler.Handle(new LoginCommand {
                Username = "admin",
                Password = "admin123"
            }, It.IsAny<CancellationToken>());

            // Assert
            userRepo.Verify(x => x.Find(It.IsAny<Expression<Func<UserModel, bool>>>()), Times.Once);
        }
    }
}