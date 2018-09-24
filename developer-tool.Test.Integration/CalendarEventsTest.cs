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

namespace TestIntegration
{
    public class CalendarEventsTest : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public CalendarEventsTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("Test title event 1")]
        [InlineData("Test title event 2")]
        public async Task GetCalendarEvents_ShouldReturnAllItems(string title)
        {
            var response = await _client.GetAsync($"/api/calendar/events");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<CalendarEventViewModel>>(stringResponse).ToList();

            Assert.Equal(3, result.Count());
            Assert.Equal(1, result.Count(a => a.Title == title));
        }

        [Fact]
        public async Task GetCalendarEvents_ShouldReturnCountEquals3()
        {
            var response = await _client.GetAsync($"/api/calendar/events");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<CalendarEventViewModel>>(stringResponse).ToList();

            Assert.Equal(3, result.Count());
        }
    }
}
