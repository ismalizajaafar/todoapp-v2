using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;

public interface ITodoRepository : IRepository<TodoItem>
{
    Task<IEnumerable<TodoItem>> GetCompletedAsync();
    Task<IEnumerable<TodoItem>> GetPendingAsync();
}