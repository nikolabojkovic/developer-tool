using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using WebApi.InputModels;
using WebApi.ViewModels;
using Xunit;

namespace TestIntegration.Todo
{
    public class TodosIntegrationTest : IntegrationTestBase
    {
        private readonly HttpClient _client;
        private const bool _activeReminder = true;

        public TodosIntegrationTest() 
        {
            _client = base.GetClient();
        }

        [Fact]
        public async Task TestGetTodos_ShouldReturnCountEquals4()
        {
            // Act
            var response = await _client.GetAsync($"/api/todos");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            var eventList = JsonConvert.DeserializeObject<IEnumerable<TodoViewModel>>(result.data.ToString());

            // Assert
            Assert.Equal(4, eventList.Count);
        }

        [Theory]
        [InlineData(1, "Test todo create", false, false)]
        [InlineData(2, "Test todo completed", true, false)]
        [InlineData(3, "Test todo uncompleted", false, false)]
        [InlineData(4, "Test todo archived", true, true)]
        public async Task TestGetTodo_ById_ShouldReturnTodo(
            int id,  
            string description,
            bool isCompleted,
            bool isArchived)
        {
            var response = await _client.GetAsync($"/api/todos/{id}");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            dynamic responseResult = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            var result = JsonConvert.DeserializeObject<TodoViewModel>(responseResult.data.ToString()) as TodoViewModel;

            // Assert
            result.Id.Should().Be(id);
            result.Description.Should().Be(description);
            result.IsCompleted.Should().Be(isCompleted);
            result.IsArchived.Should().Be(isArchived);
        }

        [Theory]
        [InlineData(5, "Test post todo 1")]
        [InlineData(5, "Test post todo 2")]
        public async Task TestPostTodo_ShouldReturnNoContentResult(
            int id,
            string description)
        {
            // Arrange
            TodoInputModel inputModel = new TodoInputModel {
                Description = description
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/todos/", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await TestGetTodo_ById_ShouldReturnTodo(id, description, false, false);            
        }

        [Theory]
        [InlineData(2, "Test update todo 2.2")]
        [InlineData(3, "Test update todo 3.3")]
        public async Task TestUpateTodo_ShouldReturnNoContentResult(
            int id,
            string description)
        {
            // Arrange
            TodoInputModel inputModel = new TodoInputModel {
                Description = description
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/todos/{id}", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Check updated data
            await TestGetTodo_ById_ShouldReturnTodo(id, description, false, false);       
        }

        [Theory]
        [InlineData(1)]
        public async Task TestDeleteTodo_ById_ShouldReturnBadRequest(int id)
        {
            var response = await _client.DeleteAsync($"/api/todos/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(3, "Test todo uncompleted")]
        public async Task TestCompleteTodo_ShouldReturnNoContentResult(
            int id,
            string description)
        {
            // Arrange
            var json = "";
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/todos/complete/{id}", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Check updated data
            await TestGetTodo_ById_ShouldReturnTodo(id, description, true, false);       
        }

        [Theory]
        [InlineData(2, "Test todo completed")]
        public async Task TestUnCompleteTodo_ShouldReturnNoContentResult(
            int id,
            string description)
        {
            // Arrange
            var json = "";
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/todos/uncomplete/{id}", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Check updated data
            await TestGetTodo_ById_ShouldReturnTodo(id, description, false, false);       
        }

        [Theory]
        [InlineData(2, "Test todo completed")]
        public async Task TestArchiveTodo_ShouldReturnNoContentResult(
            int id,
            string description)
        {
            var json = "";
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/todos/archive/{id}", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Check updated data
            await TestGetTodo_ById_ShouldReturnTodo(id, description, true, true);       
        }
    }
}