using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Core.Interfaces;
using Infrastructure.Models;
using System;
using WebApi.ViewModels;
using WebApi.InputModels;
using FluentAssertions;
using Common.Enums;
using System.Net;
using System.Text;

namespace TestIntegration
{
    public class CalendarEventsTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public CalendarEventsTest() 
        {
            _client = base.GetClient();
        }

        [Fact]
        public async Task GetCalendarEvents_ShouldReturnCountEquals3()
        {
            // Act
            var response = await _client.GetAsync($"/api/calendar/events");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<CalendarEventViewModel>>(stringResponse).ToList();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Theory]
        [InlineData(1, "#00abff", "Test title event 1", "Test description event 1", "2018-9-24", "2018-9-26", "2018/9/24 08:40:00", ReminderTimeOffset.FifteenMinBefore)]
        public async Task GetCalendarEvent_ById_ShouldReturnEventWithReminder(
            int id, 
            string color, 
            string title, 
            string description,
            string start, 
            string end,
            string reminderTime, 
            ReminderTimeOffset reminderTimeOffset)
        {
            var optionalEndDate = end == null ? default(DateTime?) : DateTime.Parse(end);
            var response = await _client.GetAsync($"/api/calendar/events/{id}");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CalendarEventViewModel>(stringResponse);

            // Assert
            result.Id.Should().Be(id);
            result.Color.Should().Be(color);
            result.Title.Should().Be(title);
            result.Description.Should().Be(description);
            result.Start.Should().Be(DateTime.Parse(start));
            result.End.Should().Be(optionalEndDate);
            result.Reminder.Time.Should().Be(DateTime.Parse(reminderTime));
            result.Reminder.TimeOffset.Should().Be(reminderTimeOffset);
        }

        [Theory]
        [InlineData(2, "#00abff", "Test title event 2", "Test description event 2", "2018-9-27")]
        [InlineData(3, "#00abff", "Test title event 3", "Test description event 3", "2018-9-25", "2018-9-30")]
        public async Task GetCalendarEvent_ById_ShouldReturnEvent(
            int id, 
            string color, 
            string title, 
            string description,
            string start, 
            string end = null)
        {
            var optionalEndDate = end == null ? default(DateTime?) : DateTime.Parse(end);
            var response = await _client.GetAsync($"/api/calendar/events/{id}");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CalendarEventViewModel>(stringResponse);

            // Assert
            result.Id.Should().Be(id);
            result.Color.Should().Be(color);
            result.Title.Should().Be(title);
            result.Description.Should().Be(description);
            result.Start.Should().Be(DateTime.Parse(start));
            result.End.Should().Be(optionalEndDate);
        }

        [Theory]
        [InlineData("#00abff", "Test post title event 1", "Test post description event 1", "2018-9-21", "2018-9-23")]
        [InlineData("#00abff", "Test post title event 2", "Test post description event 2", "2018-9-23")]
        public async Task PostCalendarEvent_ShouldReturnNoContentResult(
            string color, 
            string title, 
            string description,
            string start, 
            string end = null)
        {
            // Arrange
            CalendarEventInputModel inputModel = new CalendarEventInputModel {
                Color = color,
                Title = title,
                Description = description,
                Start = DateTime.Parse(start),
                End = end == null ? default(DateTime) : DateTime.Parse(end)
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/calendar/events/", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);            
        }

        [Theory]
        [InlineData("#00abff", "Test post title event 1", "Test post description event 1", "2018-9-21", "2018-9-23", "2018/9/21 08:40:00", ReminderTimeOffset.FifteenMinBefore)]
        public async Task PostCalendarEvent_WithReminder_ShouldReturnNoContentResult(
            string color, 
            string title, 
            string description,
            string start, 
            string end,
            string reminderTime, 
            ReminderTimeOffset reminderTimeOffset)
        {
            // Arrange
            CalendarEventInputModel inputModel = new CalendarEventInputModel {
                Color = color,
                Title = title,
                Description = description,
                Start = DateTime.Parse(start),
                End = end == null ? default(DateTime) : DateTime.Parse(end),
                Reminder = new ReminderInputModel {
                    Time = DateTime.Parse(reminderTime),
                    TimeOffset = reminderTimeOffset
                }
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/calendar/events/", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);            
        }

        [Theory]
        [InlineData(1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-21", "2018-9-24")]
        [InlineData(2, "#00abff", "Test update title event 2.2", "Test update description event 2.2", "2018-9-22")]
        public async Task UpateCalendarEvent_ShouldReturnNoContentResult(
            int id,
            string color, 
            string title, 
            string description,
            string start, 
            string end = null)
        {
            // Arrange
            CalendarEventInputModel inputModel = new CalendarEventInputModel {
                Id = id,
                Color = color,
                Title = title,
                Description = description,
                Start = DateTime.Parse(start),
                End = end == null ? default(DateTime?) : DateTime.Parse(end)
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/calendar/events/", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Check updated data
            await GetCalendarEvent_ById_ShouldReturnEvent(id, color, title, description, start, end);      
        }

        [Theory]
        [InlineData(1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-22", null, 1, "2018/9/22 09:40:00", ReminderTimeOffset.FifteenMinBefore)]
        [InlineData(1, "#00abff", "Test update title event 1.1", "Test update description event 1.1", "2018-9-22", "2018-9-23", 1, "2018/9/22 09:40:00", ReminderTimeOffset.ThirtyMinBefore)]
        public async Task UpdateCalendarEvent_WithReminder_ShouldReturnNoContentResult(
            int id, 
            string color, 
            string title, 
            string description,
            string start, 
            string end,
            int reminderId,
            string reminderTime, 
            ReminderTimeOffset reminderTimeOffset)
        {
            // Arrange
            CalendarEventInputModel inputModel = new CalendarEventInputModel {
                Id = id,
                Color = color,
                Title = title,
                Description = description,
                Start = DateTime.Parse(start),
                End = end == null ? default(DateTime?) : DateTime.Parse(end),
                Reminder = new ReminderInputModel {
                    Id = id,
                    Time = DateTime.Parse(reminderTime),
                    TimeOffset = reminderTimeOffset
                }
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/calendar/events/", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Check updated data
            await GetCalendarEvent_ById_ShouldReturnEventWithReminder(id, color, title, description, start, end, reminderTime, reminderTimeOffset);   
        }
    }
}
