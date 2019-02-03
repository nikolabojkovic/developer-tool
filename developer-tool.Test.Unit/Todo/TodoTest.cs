using Xunit;
using Domain.Models;
using FluentAssertions;

namespace TestUnit.TodoTests
{
    public class TodoTest 
    {
        [Theory]
        [InlineData("Test todo 1")]
        public void TodoTest_CreateTodo_ShouldReturnTodo(string description)
        {
            var todo = Todo.Create(description);
            todo.Description.Should().Be(description);
        }

        [Theory]
        [InlineData("Test todo 1")]
        public void TodoTest_CreateTodo_ShouldReturnTodoUnCompleted(string description)
        {
            var todo = Todo.Create(description);
            todo.IsCompleted.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test todo 1")]
        public void TodoTest_CreateTodo_ShouldReturnTodoUnArchived(string description)
        {
            var todo = Todo.Create(description);
            todo.IsArchived.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test todo 1")]
        public void TodoTest_CompleteTodo_ShouldMarkTodoAsCompleted(string description)
        {
            var todo = Todo.Create(description);
            todo.Complete();
            todo.IsCompleted.Should().BeTrue();
        }

        [Theory]
        [InlineData("Test todo 1")]
        public void TodoTest_UnCompleteTodo_ShouldMarkTodoAsUnCompleted(string description)
        {
            var todo = Todo.Create(description);
            todo.UnComplete();
            todo.IsCompleted.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test todo 1")]
        public void TodoTest_CompleteTodo_ShouldMarkTodoAsArchived(string description)
        {
            var todo = Todo.Create(description);
            todo.Archive();
            todo.IsArchived.Should().BeTrue();
        }
    } 
}