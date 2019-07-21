using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using System;
using WebApi.InputModels;
using FluentAssertions;
using System.Net;
using System.Text;

namespace TestIntegration
{
    public class CalendarEventsFailedTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public CalendarEventsFailedTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData(4)]
        public async Task GetCalendarEvent_ById_ShouldReturnNoContentFound(int id)
        {
            var response = await _client.GetAsync($"/api/calendar/events/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent); 
        }

        [Fact]
        public async Task PostCalendarEvent_ShouldReturnBadReqeust()
        {
            var json = JsonConvert.SerializeObject(null);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/calendar/events/", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);            
        }

        [Fact]
        public async Task UpdateCalendarEvent_ShouldReturnBadReqeust()
        {
            var json = JsonConvert.SerializeObject(null);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var id = 1;

            // Act
            var response = await _client.PutAsync($"/api/calendar/events/{id}", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);            
        }

        [Theory]
        [InlineData(4, "#00abff", "Test title event 2", "Test description event 2", "2018-9-27")]
        public async Task UpdateCalendarEvent_ShouldReturnNotFound(
            int id,
            string color, 
            string title, 
            string description,
            string start, 
            string end = null
        )
        {
            CalendarEventInputModel inputModel = new CalendarEventInputModel {
                Color = color,
                Title = title,
                Description = description,
                Start = DateTime.Parse(start),
                End = end == null ? default(DateTime) : DateTime.Parse(end)
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/calendar/events/{id}", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

        [Theory]
        [InlineData(4)]
        public async Task DeleteCalendarEvent_ById_ShouldReturnNotFound(int id)
        {
            var response = await _client.DeleteAsync($"/api/calendar/events/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }
    }
}
