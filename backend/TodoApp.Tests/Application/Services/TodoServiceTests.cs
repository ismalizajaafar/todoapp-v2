using Moq;
using TodoApp.Application.DTOs;
using TodoApp.Application.Services;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.Tests.Application.Services;

public class TodoServiceTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly TodoService _todoService;

    public TodoServiceTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _todoService = new TodoService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidData_ReturnsTodoItemDto()
    {
        // Arrange
        var createDto = new CreateTodoItemDto 
        { 
            Title = "Test Todo", 
            Description = "Test Description" 
        };
        
        var todoItem = new TodoItem 
        { 
            Id = 1, 
            Title = "Test Todo", 
            Description = "Test Description" 
        };

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
                      .ReturnsAsync(todoItem);

        // Act
        var result = await _todoService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal(createDto.Description, result.Description);
        Assert.False(result.IsCompleted);
    }

    [Fact]
    public async Task CreateAsync_EmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var createDto = new CreateTodoItemDto { Title = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _todoService.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_ValidData_ReturnsUpdatedTodoItem()
    {
        // Arrange
        var id = 1;
        var updateDto = new UpdateTodoItemDto 
        { 
            Title = "Updated Title", 
            Description = "Updated Description",
            IsCompleted = true 
        };
        
        var existingItem = new TodoItem 
        { 
            Id = id, 
            Title = "Original Title", 
            Description = "Original Description",
            IsCompleted = false 
        };

        _mockRepository.Setup(repo => repo.GetByIdAsync(id))
                      .ReturnsAsync(existingItem);
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>()))
                      .Returns(Task.CompletedTask);

        // Act
        var result = await _todoService.UpdateAsync(id, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.Title, result.Title);
        Assert.Equal(updateDto.Description, result.Description);
        Assert.True(result.IsCompleted);
    }
}