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
        public void TestCalendarService_GetAll_ShouldCallFindAllRepositoryAndAutoMapper()
        {
            // Arrange
            var expectedRepositoryEvents = new EventModel[] { };
            Mock<IRepository<EventModel>> calendarEventRepository = new Mock<IRepository<EventModel>>();
                                     calendarEventRepository.Setup(x => x.FindAll())
                                                            .Returns(expectedRepositoryEvents.AsQueryable());
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<IEnumerable<CalendarEvent>>(It.IsAny<IEnumerable<EventModel>>()))
                          .Returns(It.IsAny<IEnumerable<CalendarEvent>>());

            var testService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);
            
            // Act
            var results = testService.GetAllData();

            // Assert
            calendarEventRepository.Verify(x => x.FindAll(), Times.Once);
            mapperMock.Verify(x => x.Map<IEnumerable<CalendarEvent>>(It.IsAny<IEnumerable<EventModel>>()), Times.Once);
        }

        [Fact]
        public void TestCalendarService_GetById_ShouldCallFindRepositoryAndAutoMapper()
        {
            // Arrange
            var expectedRepositoryEvents = new EventModel[] { };
            // mock repository
            Mock<IRepository<EventModel>> calendarEventRepository = new Mock<IRepository<EventModel>>();
            calendarEventRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<EventModel, bool>>>()))
                                   .Returns(expectedRepositoryEvents.AsQueryable());
            // mock automapper            
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEvent>(It.IsAny<EventModel>()))
                          .Returns(It.IsAny<CalendarEvent>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            var result = calendarService.GetById(1);
      
            // Assert
            calendarEventRepository.Verify(x => x.Find(It.IsAny<Expression<Func<EventModel, bool>>>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEvent>(It.IsAny<EventModel>()), Times.Once);
        }
    
        [Fact]
        public void TestCalendarService_Store_ShouldCallAddRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<EventModel>> calendarEventRepository = new Mock<IRepository<EventModel>>();
            calendarEventRepository.Setup(repo => repo.Add(It.IsAny<EventModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<EventModel>(It.IsAny<CalendarEvent>()))
                          .Returns(It.IsAny<EventModel>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Store(It.IsAny<CalendarEvent>());
      
            // Assert
            calendarEventRepository.Verify(x => x.Add(It.IsAny<EventModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<EventModel>(It.IsAny<CalendarEvent>()), Times.Once);
        }

        [Fact]  
        public void TestCalendarService_Update_ShouldCallUpdateRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<EventModel>> calendarEventRepository = new Mock<IRepository<EventModel>>();
            calendarEventRepository.Setup(repo => repo.Update(It.IsAny<EventModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEvent, EventModel>(It.IsAny<CalendarEvent>(),It.IsAny<EventModel>()))
                          .Returns(It.IsAny<EventModel>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Update(It.IsAny<CalendarEvent>());
      
            // Assert
            calendarEventRepository.Verify(x => x.Update(It.IsAny<EventModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEvent, EventModel>(It.IsAny<CalendarEvent>(), It.IsAny<EventModel>()), Times.Once);
        }    

        [Fact]  
        public void TestCalendarService_Delete_ShouldCallDeleteRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<EventModel>> calendarEventRepository = new Mock<IRepository<EventModel>>();
            calendarEventRepository.Setup(repo => repo.Delete(It.IsAny<EventModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<EventModel>(It.IsAny<CalendarEvent>()))
                          .Returns(It.IsAny<EventModel>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Remove(1);
      
            // Assert
            calendarEventRepository.Verify(x => x.Delete(It.IsAny<EventModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<EventModel>(It.IsAny<CalendarEvent>()), Times.Never);
        }  
    }
}
