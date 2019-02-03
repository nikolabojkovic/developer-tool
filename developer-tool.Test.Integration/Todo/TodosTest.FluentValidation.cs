using Newtonsoft.Json;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TestIntegration {

    public class TodosFluentValidationTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public TodosFluentValidationTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("Todo description is required", 1, null)]
        public async Task TestUpdateTodo_ShouldReturnBadReqeust(
            string errorMessage,
            int id, 
            string description)
        {
            // Arrange
            dynamic inputModel = new { 
                Description = description
            };

            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/todos/{id}", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            stringResponse.Contains(errorMessage).Should().BeTrue();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}