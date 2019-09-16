using Xunit;
using Domain.Models;
using FluentAssertions;
using WebApi.InputModels;
using FluentValidation;
using FluentValidation.Results;

namespace TestUnit.UserTests
{
    public class UserValidationTest 
    {
        [Theory]
        [InlineData("username1", "pASSWORD1", true)]
        [InlineData("username1", "pA", false)]
        [InlineData("us", "pASSWORD1", false)]
        [InlineData("use#", "pASSWORD1", false)]
        public void Login_Validation_ShouldValidate(string username, string password, bool valid)
        {
            LoginInputModel loginInput = new LoginInputModel {
                Username = username,
                Password = password
            };

            LoginValidator validator = new LoginValidator();

            ValidationResult result = validator.Validate(loginInput);
            result.IsValid.Should().Be(valid);
        }

        [Theory]
        [InlineData("username1", "pASSWORD1", "FirstName", "LastName", true)]
        [InlineData("username1", "pA3", "FirstName", "LastName", false)]
        [InlineData("username1", "pAssword", "FirstName", "LastName", false)]
        [InlineData("us", "pASSWORD1", "FirstName", "LastName", false)]
        [InlineData("use#", "pASSWORD1", "FirstName", "LastName", false)]
        [InlineData("username1", "pASSWORD1", "", "", false)]
        [InlineData("username1", "pASSWORD1", " ", " ", false)]
        [InlineData("username1", "pASSWORD1", "_324", "_ awf", false)]
        [InlineData("username1", "pASSWORD1", "firstNamefirstNamefirstNamefirstNamefirstNamefirstNamefirstNamefirstNamefirstNamefirstNamefirstNamefirstName", "", false)]
        public void Register_Validation_ShouldValidate(string username, string password, string firstName, string lastName, bool valid)
        {
            RegisterInputModel registerInput = new RegisterInputModel {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            RegisterValidator validator = new RegisterValidator();

            ValidationResult result = validator.Validate(registerInput);
            result.IsValid.Should().Be(valid);
        }
    } 
}