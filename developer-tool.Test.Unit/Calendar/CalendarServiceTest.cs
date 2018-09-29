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
    public class CalendarServiceTest
    {
        [Fact]
        public void CalendarServiceGetAll_Events_ShouldCallFindAllRepositoryAndAutoMapper()
        {
            // Arrange
            var expectedRepositoryEvents = new Event[] { };
            Mock<IRepository<Event>> calendarEventRepository = new Mock<IRepository<Event>>();
                                     calendarEventRepository.Setup(x => x.FindAll())
                                                            .Returns(expectedRepositoryEvents.AsQueryable());
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<IEnumerable<CalendarEvent>>(It.IsAny<IEnumerable<Event>>()))
                          .Returns(It.IsAny<IEnumerable<CalendarEvent>>());

            var testService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);
            
            // Act
            var results = testService.GetAllData();

            // Assert
            calendarEventRepository.Verify(x => x.FindAll(), Times.Once);
            mapperMock.Verify(x => x.Map<IEnumerable<CalendarEvent>>(It.IsAny<IEnumerable<Event>>()), Times.Once);
        }

        [Fact]
        public void CalendarServiceGet_ById_ShouldCallFindRepositoryAndAutoMapper()
        {
            // Arrange
            var expectedRepositoryEvents = new Event[] { };
            // mock repository
            Mock<IRepository<Event>> calendarEventRepository = new Mock<IRepository<Event>>();
            calendarEventRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Event, bool>>>()))
                                   .Returns(expectedRepositoryEvents.AsQueryable());
            // mock automapper            
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEvent>(It.IsAny<Event>()))
                          .Returns(It.IsAny<CalendarEvent>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            var result = calendarService.GetById(1);
      
            // Assert
            calendarEventRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Event, bool>>>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEvent>(It.IsAny<Event>()), Times.Once);
        }
    
        [Fact]
        public void CalendarServicePost_Event_ShouldCallAddRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<Event>> calendarEventRepository = new Mock<IRepository<Event>>();
            calendarEventRepository.Setup(repo => repo.Add(It.IsAny<Event>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<Event>(It.IsAny<CalendarEvent>()))
                          .Returns(It.IsAny<Event>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Store(It.IsAny<CalendarEvent>());
      
            // Assert
            calendarEventRepository.Verify(x => x.Add(It.IsAny<Event>()), Times.Once);
            mapperMock.Verify(x => x.Map<Event>(It.IsAny<CalendarEvent>()), Times.Once);
        }

        [Fact]  
        public void CalendarServiceUpdate_Event_ShouldCallUpdateRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<Event>> calendarEventRepository = new Mock<IRepository<Event>>();
            calendarEventRepository.Setup(repo => repo.Update(It.IsAny<Event>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEvent, Event>(It.IsAny<CalendarEvent>(),It.IsAny<Event>()))
                          .Returns(It.IsAny<Event>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Update(It.IsAny<CalendarEvent>());
      
            // Assert
            calendarEventRepository.Verify(x => x.Update(It.IsAny<Event>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEvent, Event>(It.IsAny<CalendarEvent>(), It.IsAny<Event>()), Times.Once);
        }    

        [Fact]  
        public void CalendarServiceDelete_Event_ShouldCallDeleteRepositoryAndAutoMapper()
        {
             // Arrange
            // mock repository
            Mock<IRepository<Event>> calendarEventRepository = new Mock<IRepository<Event>>();
            calendarEventRepository.Setup(repo => repo.Delete(It.IsAny<Event>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<Event>(It.IsAny<CalendarEvent>()))
                          .Returns(It.IsAny<Event>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Remove(1);
      
            // Assert
            calendarEventRepository.Verify(x => x.Delete(It.IsAny<Event>()), Times.Once);
            mapperMock.Verify(x => x.Map<Event>(It.IsAny<CalendarEvent>()), Times.Never);
        }  
    }
}
