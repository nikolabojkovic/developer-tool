using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using WebApi.InputModels;
using Xunit;

namespace TestIntegration
{
    public class RegisterUserFailTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public RegisterUserFailTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("Username: {0} is not available, plese choose other username.", "admin", "test123", "fTest", "lTest")]
        public async Task Post_RegisterUser_ShouldReturnNoContentResult(
            string errorMessage,
            string username, 
            string password, 
            string firstName,
            string lastName)
        {
            // Arrange
            RegisterInputModel inputModel = new RegisterInputModel {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/authentication/register/", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            stringResponse.Contains(string.Format(errorMessage, username)).Should().BeTrue();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);          
        }
    }
}