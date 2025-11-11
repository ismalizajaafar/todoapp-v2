using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.Title))
            throw new ArgumentException("Title is required");

        var todoItem = new TodoItem
        {
            Title = createDto.Title.Trim(),
            Description = createDto.Description?.Trim() ?? string.Empty,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _todoRepository.AddAsync(todoItem);
        return MapToDto(created);
    }

    public async Task DeleteAsync(int id)
    {
        var todoItem = await _todoRepository.GetByIdAsync(id);
        if (todoItem != null)
        {
            await _todoRepository.DeleteAsync(todoItem);
        }
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllAsync()
    {
        var items = await _todoRepository.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id)
    {
        var item = await _todoRepository.GetByIdAsync(id);
        return item == null ? null : MapToDto(item);
    }

    public async Task<IEnumerable<TodoItemDto>> GetCompletedAsync()
    {
        var items = await _todoRepository.GetCompletedAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<TodoItemDto>> GetPendingAsync()
    {
        var items = await _todoRepository.GetPendingAsync();
        return items.Select(MapToDto);
    }

    public async Task<TodoItemDto> UpdateAsync(int id, UpdateTodoItemDto updateDto)
    {
        if (string.IsNullOrWhiteSpace(updateDto.Title))
            throw new ArgumentException("Title is required");

        var existingItem = await _todoRepository.GetByIdAsync(id);
        if (existingItem == null)
            throw new KeyNotFoundException($"Todo item with ID {id} not found");

        existingItem.Title = updateDto.Title.Trim();
        existingItem.Description = updateDto.Description?.Trim() ?? string.Empty;
        existingItem.IsCompleted = updateDto.IsCompleted;
        existingItem.UpdatedAt = DateTime.UtcNow;

        await _todoRepository.UpdateAsync(existingItem);
        return MapToDto(existingItem);
    }

    private static TodoItemDto MapToDto(TodoItem item)
    {
        return new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsCompleted = item.IsCompleted,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}