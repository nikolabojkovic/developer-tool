using System;
using System.Linq;
using System.Linq.Expressions;
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
        public void TestServiceGet_AllData_ShouldRegurnCount2()
        {
            // Arrange
            var testArray = new Test[]
            {
                new Test { FirstName = "Test 1" },
                new Test { FirstName = "Test 2" }
            };
            Mock<IRepository<Test>> testRepository = new Mock<IRepository<Test>>();
            testRepository.Setup(x => x.FindAll()).Returns(testArray.AsQueryable());
            var testService = new TestService(testRepository.Object);
            
            // Act
            var results = testService.GetAllData().ToList();

            // Assert
            Assert.Equal(2, results.Count());
            testRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, "Test 1")]
        [InlineData(2, "Test 2")]
        public void TestServiceGet_ById_ShouldRegurnTestObject(int id, string firstName)
        {
            // Arrange
            var test = new Test { Id = id, FirstName = firstName };
            var testArray = new Test[] {
                test
            };
            Mock<IRepository<Test>> testRepository = new Mock<IRepository<Test>>();
            testRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Test, bool>>>())).Returns(testArray.AsQueryable());
            var testService = new TestService(testRepository.Object);
            
            // Act
            var result = testService.GetById(id);
      
            // Assert
            Assert.Equal(firstName, result.FirstName);
            testRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Test, bool>>>()), Times.Once);
        }
    }
}
