using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Moq;
using Core.Interfaces;
using Domain.Services;
using Infrastructure.Models;
using Common.Enums;

namespace TestUnit
{
    public class CalendarTest
    {
        [Fact]
        public void CalendarServiceGet_GetAll_ShouldRegurnAllEvents()
        {
            // Arrange
            var calendarEventArray = new CalendarEvent[]
            {
                CalendarEvent.Create("#00abff", "Test title event", "Test description event", new DateTime(2018, 9, 24)),
                CalendarEvent.Create(
                    "#00abff", "Test title event 2", 
                    "Test description event 2", 
                    new DateTime(2018, 9, 24), 
                    new DateTime(2018, 9, 26), 
                    true, 
                    new DateTime(2018, 9, 24, 8, 40, 0),
                    EventDateTimeOffset.AtTimeOfEvent)
            };
            Mock<IRepository<CalendarEvent>> calendarEventRepository = new Mock<IRepository<CalendarEvent>>();
            calendarEventRepository.Setup(x => x.FindAll())
                                   .Returns(calendarEventArray.AsQueryable());
            var testService = new CalendarService(calendarEventRepository.Object);
            
            // Act
            var results = testService.GetAllData().ToList();

            // Assert
            Assert.Equal(2, results.Count());
            calendarEventRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, "#00abff", "Test title event", "Test description event", "2018-9-24")]
        [InlineData(2, "#00abff", "Test title event 2", "Test description event2", "2018-9-26")]
        public void CalendarServiceGet_ById_ShouldRegurnEvent(int id, string color, string title, string description, string start)
        {
            // Arrange
            var calendarEvent = CalendarEvent.Create(color, title, description, DateTime.Parse(start));
            calendarEvent.Id = id;
            var calendarEventArray = new CalendarEvent[] {
                calendarEvent
            };

            Mock<IRepository<CalendarEvent>> calendarEventRepository = new Mock<IRepository<CalendarEvent>>();
            calendarEventRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<CalendarEvent, bool>>>()))
                                   .Returns(calendarEventArray.AsQueryable());
            var calendarService = new CalendarService(calendarEventRepository.Object);

            // Act
            var result = calendarService.GetById(id);
      
            // Assert
            Assert.Equal(title, result.Title);
            calendarEventRepository.Verify(x => x.Find(It.IsAny<Expression<Func<CalendarEvent, bool>>>()), Times.Once);
        }
    }
}
