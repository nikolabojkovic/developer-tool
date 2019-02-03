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
            var expectedRepositoryEvents = new CalendarEventModel[] { };
            Mock<IRepository<CalendarEventModel>> calendarEventRepository = new Mock<IRepository<CalendarEventModel>>();
                                     calendarEventRepository.Setup(x => x.FindAll())
                                                            .Returns(expectedRepositoryEvents.AsQueryable());
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<IEnumerable<CalendarEvent>>(It.IsAny<IEnumerable<CalendarEventModel>>()))
                          .Returns(It.IsAny<IEnumerable<CalendarEvent>>());

            var testService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);
            
            // Act
            var results = testService.GetAllData();

            // Assert
            calendarEventRepository.Verify(x => x.FindAll(), Times.Once);
            mapperMock.Verify(x => x.Map<IEnumerable<CalendarEvent>>(It.IsAny<IEnumerable<CalendarEventModel>>()), Times.Once);
        }

        [Fact]
        public void TestCalendarService_GetById_ShouldCallFindRepositoryAndAutoMapper()
        {
            // Arrange
            var expectedRepositoryEvents = new CalendarEventModel[] { };
            // mock repository
            Mock<IRepository<CalendarEventModel>> calendarEventRepository = new Mock<IRepository<CalendarEventModel>>();
            calendarEventRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<CalendarEventModel, bool>>>()))
                                   .Returns(expectedRepositoryEvents.AsQueryable());
            // mock automapper            
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEvent>(It.IsAny<CalendarEventModel>()))
                          .Returns(It.IsAny<CalendarEvent>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            var result = calendarService.GetById(1);
      
            // Assert
            calendarEventRepository.Verify(x => x.Find(It.IsAny<Expression<Func<CalendarEventModel, bool>>>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEvent>(It.IsAny<CalendarEventModel>()), Times.Once);
        }
    
        [Fact]
        public void TestCalendarService_Store_ShouldCallAddRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<CalendarEventModel>> calendarEventRepository = new Mock<IRepository<CalendarEventModel>>();
            calendarEventRepository.Setup(repo => repo.Add(It.IsAny<CalendarEventModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEventModel>(It.IsAny<CalendarEvent>()))
                          .Returns(It.IsAny<CalendarEventModel>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Store(It.IsAny<CalendarEvent>());
      
            // Assert
            calendarEventRepository.Verify(x => x.Add(It.IsAny<CalendarEventModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEventModel>(It.IsAny<CalendarEvent>()), Times.Once);
        }

        [Fact]  
        public void TestCalendarService_Update_ShouldCallUpdateRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<CalendarEventModel>> calendarEventRepository = new Mock<IRepository<CalendarEventModel>>();
            calendarEventRepository.Setup(repo => repo.Update(It.IsAny<CalendarEventModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEvent, CalendarEventModel>(It.IsAny<CalendarEvent>(),It.IsAny<CalendarEventModel>()))
                          .Returns(It.IsAny<CalendarEventModel>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Update(It.IsAny<CalendarEvent>());
      
            // Assert
            calendarEventRepository.Verify(x => x.Update(It.IsAny<CalendarEventModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEvent, CalendarEventModel>(It.IsAny<CalendarEvent>(), It.IsAny<CalendarEventModel>()), Times.Once);
        }    

        [Fact]  
        public void TestCalendarService_Delete_ShouldCallDeleteRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<CalendarEventModel>> calendarEventRepository = new Mock<IRepository<CalendarEventModel>>();
            calendarEventRepository.Setup(repo => repo.Delete(It.IsAny<CalendarEventModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<CalendarEventModel>(It.IsAny<CalendarEvent>()))
                          .Returns(It.IsAny<CalendarEventModel>());

            var calendarService = new CalendarService(calendarEventRepository.Object, mapperMock.Object);

            // Act
            calendarService.Remove(1);
      
            // Assert
            calendarEventRepository.Verify(x => x.Delete(It.IsAny<CalendarEventModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<CalendarEventModel>(It.IsAny<CalendarEvent>()), Times.Never);
        }  
    }
}
