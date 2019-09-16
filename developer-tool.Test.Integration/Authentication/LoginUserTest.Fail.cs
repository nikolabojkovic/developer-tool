using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Authentication.Commands;
using FluentAssertions;
using Newtonsoft.Json;
using WebApi.InputModels;
using Xunit;

namespace TestIntegration
{
    public class LoginUserFailTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public LoginUserFailTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("Wrong credentials.", "admin", "teSt12345")]
        [InlineData("Password must contain at least lovercase and upercase letter and number.", "admin", "test12345")]
        public async Task Post_LoginUser_ShouldReturnBadRequest(
            string errorMessage,
            string username, 
            string password)
        {
            // Arrange
            LoginInputModel inputModel = new LoginInputModel {
                Username = username,
                Password = password
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/authentication/login/", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            stringResponse.Contains(errorMessage).Should().BeTrue();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);         
        }
    }
}