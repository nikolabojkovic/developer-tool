using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using WebApi.InputModels;
using FluentAssertions;
using System.Net;
using System.Text;

namespace TestIntegration
{
    public class TodosFailedTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public TodosFailedTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData(5)]
        public async Task TestGetTodo_ById_ShouldReturnNotFound(int id)
        {
            var response = await _client.GetAsync($"/api/todos/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

        [Fact]
        public async Task TestPostTodo_ShouldReturnBadReqeust()
        {
            var json = JsonConvert.SerializeObject(null);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/todos/", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);            
        }

        [Fact]
        public async Task TestUpdateTodo_ShouldReturnBadReqeust()
        {
            var json = JsonConvert.SerializeObject(null);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var id = 1;

            // Act
            var response = await _client.PutAsync($"/api/todos/{id}", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);            
        }

        [Theory]
        [InlineData(5, "Test description todo 5")]
        public async Task TestUpdateTodo_ShouldReturnNotFound(
            int id,
            string description
        )
        {
            TodoInputModel inputModel = new TodoInputModel {
                Description = description
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos/{id}", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

        [Theory]
        [InlineData(5)]
        public async Task TestDeleteTodo_ById_ShouldReturnNotFound(int id)
        {
            var response = await _client.DeleteAsync($"/api/todos/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

        [Theory]
        [InlineData(5)]
        public async Task TestCompleteTodo_ShouldReturnNotFound(
            int id
        )
        {
            var stringContent = new StringContent("", UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos/complete/{id}", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

        [Theory]
        [InlineData(5)]
        public async Task TestUnCompleteTodo_ShouldReturnNotFound(
            int id
        )
        {
            var stringContent = new StringContent("", UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos/uncomplete/{id}", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

        [Theory]
        [InlineData(5)]
        public async Task TestArchiveTodo_ShouldReturnNotFound(
            int id
        )
        {
            var stringContent = new StringContent("", UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos/archive/{id}", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }
    }
}
