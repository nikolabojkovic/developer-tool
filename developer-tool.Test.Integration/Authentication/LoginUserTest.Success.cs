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
    public class LoginUserSuccessTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public LoginUserSuccessTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("admin", "admiN123")]
        public async Task Post_LoginUser_ShouldReturnTokenViewModel(
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
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            dynamic responseResult = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            var result = JsonConvert.DeserializeObject<TokenViewModel>(responseResult.data.ToString()) as TokenViewModel;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);  
            result.Token.Should().NotBeEmpty();
            result.ExpiresIn.Should().Be(30);         
        }
    }
}