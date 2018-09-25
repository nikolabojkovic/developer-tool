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
    public class CalendarEventsBadRequestTest : IntegrationTestBase
    {
        private readonly HttpClient _client;
        private const bool reminderOn = true;
        private const bool reminderOff = false;

        public CalendarEventsBadRequestTest() 
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData(4)]
        public async Task GetCalendarEventById_ShouldNotFound(int id)
        {
            var response = await _client.GetAsync($"/api/calendar/events/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
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
    }
}
