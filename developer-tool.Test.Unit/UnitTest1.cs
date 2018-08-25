using System;
using System.Linq;
using Xunit;
using Moq;
using WebApi.Interfaces;
using WebApi.Services;
using WebApi.Models;

namespace TestUnit
{
    public class UnitTest1
    {
        [Fact]
        public void TestServiceGet_ShouldRegurnCount2()
        {
            // Arrange
            var testArray = new Test[]
            {
                new Test { FirstName = "Test 1" },
                new Test { FirstName = "Test 2" }
            };
            Mock<IRepository<Test>> testRepository = new Mock<IRepository<Test>>();
            testRepository.Setup(x => x.FindAll()).Returns(testArray.AsQueryable ());
            var testService = new TestService(testRepository.Object);
            
            // Act
            var results = testService.GetAllData().ToList();

            // Assert
            Assert.Equal(2, results.Count());
        }
    }
}
