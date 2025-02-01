using web.api.Models;

namespace web.unit.tests.todos;

public class Todo_Unit_Tests
{
    [Fact]
    public void Todo_Should_CompleteTask()
    {
        // Arrange
        Todo todo = new Todo() { Name = "Test Todo" };
        var currentStatus = todo.IsComplete;
        // Act
        todo.CompleteTask();

        // Assert
        Assert.True(currentStatus != todo.IsComplete);
    }

    [Fact]
    public void Todo_Should_BeFalse_UponCreate()
    {
        // Arrange & Act at initialization it is already incomplete
        Todo todo = new Todo() { Name = "Test Todo" };
        var currentStatus = todo.IsComplete;

        // Assert
        Assert.False(currentStatus);
    }

    [Fact]
    public void Todo_Should_ThrowInvalidError_WhenTryToComplete()
    {
        // Arrange
        Todo todo = new Todo() { Name = "Test Todo" };
        var currentStatus = todo.IsComplete;
        // Act
        todo.CompleteTask(); // Tagged as Completed first time

        // Assert that InvalidOperationException is thrown if we try to complete the task again
        Assert.Equal(
            "Task is already completed",
            Assert.Throws<InvalidOperationException>(() => todo.CompleteTask()).Message
        );
    }

    [Fact]
    public void Todo_Should_ThrowInvalidError_WhenTryToInComplete()
    {
        // Arrange & Act at initialization it is already incomplete
        Todo todo = new Todo() { Name = "Test Todo" };

        // Assert that InvalidOperationException is thrown if we try to complete the task again
        Assert.Equal(
            "Task is already incomplete",
            Assert.Throws<InvalidOperationException>(() => todo.IncompleteTask()).Message
        );
    }
}
