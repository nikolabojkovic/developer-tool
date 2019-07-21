using Xunit;
using Domain.Models;
using FluentAssertions;

namespace TestUnit.UserTests
{
    public class UserTest 
    {
        [Theory]
        [InlineData("username1", "password1", "firstName1", "lastName1")]
        public void Create_NewUser_ShouldReturnUserModel(string username, string password, string firstName, string lastName)
        {
            var user = User.Create(username, password, firstName, lastName);
            user.Username.Should().Be(username);
            user.Password.Should().Be(password);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
        }
    } 
}