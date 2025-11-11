using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoItemDto>> GetAllAsync();
    Task<TodoItemDto?> GetByIdAsync(int id);
    Task<TodoItemDto> CreateAsync(CreateTodoItemDto createDto);
    Task<TodoItemDto> UpdateAsync(int id, UpdateTodoItemDto updateDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<TodoItemDto>> GetCompletedAsync();
    Task<IEnumerable<TodoItemDto>> GetPendingAsync();
}