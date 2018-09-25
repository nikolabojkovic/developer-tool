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
        private const bool reminderOn = true;
        private const bool reminderOff = false;

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
        [InlineData(1, "#00abff", "Test title event 1", "Test description event 1", "2018-9-24", "2018-9-26", reminderOn, "2018/9/24 08:40:00", EventDateTimeOffset.FifteenMinBefore)]
        [InlineData(2, "#00abff", "Test title event 2", "Test description event 2", "2018-9-27", null, reminderOff, null, null)]
        [InlineData(3, "#00abff", "Test title event 3", "Test description event 3", "2018-9-25", "2018-9-30", reminderOff, null, null)]
        public async Task GetCalendarEventById_ShouldReturnEvent(
            int id, 
            string color, 
            string title, 
            string description,
            string start, 
            string end, 
            bool isReminderEnabled,
            string reminderTime, 
            EventDateTimeOffset? reminderTimeOffset)
        {
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
            result.IsReminderEnabled.Should().Be(isReminderEnabled);

            // Assert optionals
            if (end == null) result.End.Should().BeNull(); else result.End.Should().Be(DateTime.Parse(end));
            if (reminderTime == null) result.ReminderTime.Should().BeNull(); else result.ReminderTime.Should().Be(DateTime.Parse(reminderTime));
            if (reminderTimeOffset == null) result.ReminderTimeOffset.Should().BeNull(); else result.ReminderTimeOffset.Should().Be(reminderTimeOffset);
        }

        [Theory]
        [InlineData("#00abff", "Test post title event 1", "Test post description event 1", "2018-9-21", "2018-9-23", reminderOn, "2018/9/21 08:40:00", EventDateTimeOffset.FifteenMinBefore)]
        [InlineData("#00abff", "Test post title event 2", "Test post description event 2", "2018-9-22", null, reminderOff, null, null)]
        [InlineData("#00abff", "Test post title event 3", "Test post description event 3", "2018-9-24", "2018-9-27", reminderOff, null, null)]
        public async Task PostCalendarEvent_ShouldReturnNoContentResult(
            string color, 
            string title, 
            string description,
            string start, 
            string end, 
            bool isReminderEnabled,
            string reminderTime, 
            EventDateTimeOffset? reminderTimeOffset)
        {
            // Arrange
            CalendarEventInputModel inputModel = new CalendarEventInputModel {
                Color = color,
                Title = title,
                Description = description,
                Start = start == null ? default(DateTime) : DateTime.Parse(start),
                End = end == null ? default(DateTime) : DateTime.Parse(end),
                IsReminderEnabled = isReminderEnabled,
                ReminderTime = reminderTime == null ? default(DateTime) : DateTime.Parse(reminderTime),
                ReminderTimeOffset = reminderTimeOffset
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/calendar/events/", stringContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);            
        }
    }
}
