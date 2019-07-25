using Newtonsoft.Json;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Text;
using System;

namespace TestIntegration {

    public class CalendarEventsFluentValidationTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public CalendarEventsFluentValidationTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("Title is required", 1, "#00abff", "", "Test update description event 1.1", "2018-9-22", null, "2018/9/22 09:40:00", "true", "1")]
        [InlineData("The length of 'Title' must be 50 characters or fewer. You entered 74 characters.", 1, "#00abff", "Too long title too long title too long title too long title too long title", "Test update description event 1.1", "2018-9-22", null, "2018/9/22 09:40:00", "true", "1")]
        [InlineData("Start date is required", 1, "#00abff", "title test", "Test update description event 1.1", "0001-1-1", null, "2018/9/22 09:40:00", "true", "1")]
        [InlineData("The input was not valid.", 1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "", "2018-9-23", "2018/9/22 09:40:00", "true", "2")]
        [InlineData("Reminder time is required", 1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-22", "2018-9-23", "0001/1/1 00:00:00", "true", "2")]
        [InlineData("The input was not valid.", 1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-22", "2018-9-23", "", "false", "3")]
        [InlineData("The input was not valid.", 1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-22", "2018-9-23", "2018/9/22 09:40:00", "", "3")]
        [InlineData("The input was not valid.", 1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-22", "2018-9-23", "2018/9/22 09:40:00", "true", "")]
        [InlineData("Reminder offset is not valid", 1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-22", "2018-9-23", "2018/9/22 09:40:00", "true", "7")]
        public async Task UpdateCalendarEvent_ShouldReturnBadReqeust(
            string errorMessage,
            int id, 
            string color, 
            string title, 
            string description,
            string start, 
            string end,
            string reminderTime, 
            string isReminderActive,
            string reminderTimeOffset)
        {
            // Arrange
            dynamic inputModel = new { 
                Color = color,
                Title = title,
                Description = description,
                Start = start,
                End = end,
                Reminder = new {
                    Time = reminderTime,
                    Active = isReminderActive,
                    TimeOffset = reminderTimeOffset
                }
            };

            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/calendar/events/{id}", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(stringResponse);
            stringResponse.Contains(errorMessage).Should().BeTrue();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}