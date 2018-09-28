using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Moq;
using Core.Interfaces;
using Domain.Services;
using Domain.Models;
using Infrastructure.Models;
using Common.Enums;
using Infrastructure.Data;
using AutoMapper;

namespace TestUnit
{
    public class CalendarTest
    {
        [Fact]
        public void CalendarServiceGet_GetAll_ShouldRegurnAllEvents()
        {
            // Arrange
            var expectedRepositoryEvents = new Event[]
            {
                DbInitializer.CreateEvent("#00abff", "Test title event", "Test description event", new DateTime(2018, 9, 24)),
                DbInitializer.CreateEvent(
                    "#00abff", "Test title event 2", 
                    "Test description event 2", 
                    new DateTime(2018, 9, 24), 
                    new DateTime(2018, 9, 26), 
                    DbInitializer.CreateReminder( 
                        new DateTime(2018, 9, 24, 8, 40, 0),
                        ReminderTimeOffset.AtTimeOfEvent))
            };

            var expecteDomainEvents = new List<CalendarEvent> {
                CalendarEvent.Create("#00abff", "Test title event", "Test description event", new DateTime(2018, 9, 24)),
                CalendarEvent.Create("#00abff", "Test title event 2", 
                    "Test description event 2", 
                    new DateTime(2018, 9, 24), 
                    new DateTime(2018, 9, 26)
                    ).WithReminder(Domain.Models.Reminder.Create( 
                        new DateTime(2018, 9, 24, 8, 40, 0),
                        ReminderTimeOffset.AtTimeOfEvent))
            };

            Mock<IRepository<Event>> calendarEventRepository = new Mock<IRepository<Event>>();
            calendarEventRepository.Setup(x => x.FindAll())
                                   .Returns(expectedRepositoryEvents.AsQueryable());

            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<IEnumerable<CalendarEvent>>(It.IsAny<IEnumerable<Event>>()))
                          .Returns(expecteDomainEvents);

            var testService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);
            
            // Act
            var results = testService.GetAllData().ToList();

            // Assert
            Assert.Equal(2, results.Count());
            calendarEventRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, "#00abff", "Test title event", "Test description event", "2018-9-24")]
        [InlineData(2, "#00abff", "Test title event 2", "Test description event2", "2018-9-26", "2018-9-28")]
        public void CalendarServiceGet_ById_ShouldRegurnEvent(int id, string color, string title, string description, string start, string end = null)
        {
            // Arrange
            var calendarEvent = DbInitializer.CreateEvent(color, title, description, DateTime.Parse(start));
            calendarEvent.Id = id;
            var calendarEventArray = new Event[] {
                calendarEvent
            };

            // mock repository
            Mock<IRepository<Event>> calendarEventRepository = new Mock<IRepository<Event>>();
            calendarEventRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Event, bool>>>()))
                                   .Returns(calendarEventArray.AsQueryable());

            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEvent>(It.IsAny<Event>()))
                          .Returns(CalendarEvent.Create(color, 
                                                        title,
                                                        description,
                                                        DateTime.Parse(start),
                                                        end == null ? default(DateTime) : DateTime.Parse(end))
                );

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            var result = calendarService.GetById(id);
      
            // Assert
            Assert.Equal(title, result.Title);
            calendarEventRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Event, bool>>>()), Times.Once);
        }
    }
}
