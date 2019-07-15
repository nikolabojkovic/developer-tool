using Xunit;
using Moq;
using Domain.Models;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Linq.Expressions;
using Domain.PersistenceModels;
using Application.Services;

namespace TestUnit.TodoTests.Services 
{
    public class TodoServiceTest 
    {
         [Fact]
        public void TestTodoService_GetAll_ShouldCallFindAllRepositoryAndAutoMapper()
        {
            // Arrange
            var expectedTodos = new TodoModel[] { };
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
                                     todoRepository.Setup(x => x.FindAll())
                                                            .Returns(expectedTodos.AsQueryable());
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<IEnumerable<Todo>>(It.IsAny<IEnumerable<TodoModel>>()))
                          .Returns(It.IsAny<IEnumerable<Todo>>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);
            
            // Act
            var results = todoService.GetAllData();

            // Assert
            todoRepository.Verify(x => x.FindAll(), Times.Once);
            mapperMock.Verify(x => x.Map<IEnumerable<Todo>>(It.IsAny<IEnumerable<TodoModel>>()), Times.Once);
        }

        [Fact]
        public void TestTodoService_GetById_ShouldCallFindRepositoryAndAutoMapper()
        {
            // Arrange
            var expectedTodos = new TodoModel[] { };
            // mock repository
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
            todoRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()))
                          .Returns(expectedTodos.AsQueryable());
            // mock automapper            
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<Todo>(It.IsAny<TodoModel>()))
                          .Returns(It.IsAny<Todo>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);

            // Act
            var result = todoService.GetById(1);
      
            // Assert
            todoRepository.Verify(x => x.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo>(It.IsAny<TodoModel>()), Times.Once);
        }
    
        [Fact]
        public void TestTodoService_Store_ShouldCallAddRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
            todoRepository.Setup(repo => repo.Add(It.IsAny<TodoModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<TodoModel>(It.IsAny<Todo>()))
                          .Returns(It.IsAny<TodoModel>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);

            // Act
            todoService.Store(It.IsAny<Todo>());
      
            // Assert
            todoRepository.Verify(x => x.Add(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<TodoModel>(It.IsAny<Todo>()), Times.Once);
        }

        [Fact]  
        public void TesTodoService_Update_ShouldCallUpdateRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
            todoRepository.Setup(repo => repo.Update(It.IsAny<TodoModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<Todo, TodoModel>(It.IsAny<Todo>(),It.IsAny<TodoModel>()))
                          .Returns(It.IsAny<TodoModel>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);

            // Act
            todoService.Update(It.IsAny<Todo>());
      
            // Assert
            todoRepository.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo, TodoModel>(It.IsAny<Todo>(), It.IsAny<TodoModel>()), Times.Once);
        }    

        [Fact]  
        public void TestTodoService_Delete_ShouldCallDeleteRepositoryAndAutoMapper()
        {
            // Arrange
            // mock repository
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
            todoRepository.Setup(repo => repo.Delete(It.IsAny<TodoModel>()));
            // mock automapper
            var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<TodoModel>(It.IsAny<Todo>()))
                          .Returns(It.IsAny<TodoModel>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);

            // Act
            todoService.Remove(1);
      
            // Assert
            todoRepository.Verify(x => x.Delete(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<TodoModel>(It.IsAny<Todo>()), Times.Never);
        }  

        [Fact]  
        public void TestTodoService_Complete_ShouldCompleteTodo()
        {
            // Arrange
            var expectedTodos = new TodoModel[] { 
                new TodoModel {
                    Id = 1
                }
            };
            var mappedTodoDomainModel = new Mock<Todo>();
            // mock repository
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
            todoRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()))
                          .Returns(expectedTodos.AsQueryable());
            todoRepository.Setup(repo => repo.Update(It.IsAny<TodoModel>()));
            // mock automapper  
            var mapperMock = new Mock<IMapper>();  
                mapperMock.Setup(m => m.Map<Todo>(It.IsAny<TodoModel>()))
                          .Returns(mappedTodoDomainModel.Object);
                mapperMock.Setup(m => m.Map<Todo, TodoModel>(It.IsAny<Todo>(),It.IsAny<TodoModel>()))
                          .Returns(It.IsAny<TodoModel>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);

            // Act
            todoService.Complete(1);
      
            // Assert
            todoRepository.Verify(x => x.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()), Times.Once);
            todoRepository.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo>(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo, TodoModel>(It.IsAny<Todo>(), It.IsAny<TodoModel>()), Times.Once);
            mappedTodoDomainModel.Verify(x => x.Complete(), Times.Once);
        }  

        [Fact]  
        public void TestTodoService_UnComplete_ShouldCompleteTodo()
        {
            // Arrange
            var expectedTodos = new TodoModel[] { 
                new TodoModel {
                    Id = 1,
                    IsCompleted = true
                }
            };
            var mappedTodoDomainModel = new Mock<Todo>();
            // mock repository
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
            todoRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()))
                          .Returns(expectedTodos.AsQueryable());
            todoRepository.Setup(repo => repo.Update(It.IsAny<TodoModel>()));
            // mock automapper  
            var mapperMock = new Mock<IMapper>();  
                mapperMock.Setup(m => m.Map<Todo>(It.IsAny<TodoModel>()))
                          .Returns(mappedTodoDomainModel.Object);
                mapperMock.Setup(m => m.Map<Todo, TodoModel>(It.IsAny<Todo>(),It.IsAny<TodoModel>()))
                          .Returns(It.IsAny<TodoModel>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);

            // Act
            todoService.UnComplete(1);
      
            // Assert
            todoRepository.Verify(x => x.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()), Times.Once);
            todoRepository.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo>(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo, TodoModel>(It.IsAny<Todo>(), It.IsAny<TodoModel>()), Times.Once);
            mappedTodoDomainModel.Verify(x => x.UnComplete(), Times.Once);
        }

        [Fact]  
        public void TestTodoService_Archive_ShouldCompleteTodo()
        {
            // Arrange
            var expectedTodos = new TodoModel[] { 
                new TodoModel {
                    Id = 1
                }
            };
            var mappedTodoDomainModel = new Mock<Todo>();
            // mock repository
            Mock<IRepository<TodoModel>> todoRepository = new Mock<IRepository<TodoModel>>();
            todoRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()))
                          .Returns(expectedTodos.AsQueryable());
            todoRepository.Setup(repo => repo.Update(It.IsAny<TodoModel>()));
            // mock automapper  
            var mapperMock = new Mock<IMapper>();  
                mapperMock.Setup(m => m.Map<Todo>(It.IsAny<TodoModel>()))
                          .Returns(mappedTodoDomainModel.Object);
                mapperMock.Setup(m => m.Map<Todo, TodoModel>(It.IsAny<Todo>(),It.IsAny<TodoModel>()))
                          .Returns(It.IsAny<TodoModel>());

            var todoService = new TodoService(todoRepository.Object, mapperMock.Object);

            // Act
            todoService.Archive(1);
      
            // Assert
            todoRepository.Verify(x => x.Find(It.IsAny<Expression<Func<TodoModel, bool>>>()), Times.Once);
            todoRepository.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo>(It.IsAny<TodoModel>()), Times.Once);
            mapperMock.Verify(x => x.Map<Todo, TodoModel>(It.IsAny<Todo>(), It.IsAny<TodoModel>()), Times.Once);
            mappedTodoDomainModel.Verify(x => x.Archive(), Times.Once);
        }
    }
}