using FirstTodoWebApi.Entities;

namespace FirstTodoWebApi.Interfaces;

public interface ITodoService
{
    Task<IQueryable<Todo>> GetAllAsync(Guid userId);

    Task<Todo?> GetByIdAsync(Guid id);

    Task<Todo> CreateAsync(string title, Guid userId);

    Task<Todo?> UpdateAsync(Guid todoId, Guid userId, string title, bool isCompleted);

    Task<bool> DeleteAsync(Guid todoId, Guid userId);
    Task<Todo?> ToggleTodoAsync(Guid todoId, Guid userId);
}