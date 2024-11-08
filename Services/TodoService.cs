using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstTodoWebApi.Services;

public class TodoService : ITodoService
{

    private readonly IApplicationDbContext _context;

    public TodoService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IQueryable<Todo>> GetAllAsync()
    {
        return _context.Todos;
    }

    public async Task<Todo?> GetByIdAsync(Guid id)
    {
        return await _context.Todos.FirstOrDefaultAsync(todo => todo.Id == id);
    }

    public async Task<Todo> CreateAsync(string title, Guid userId)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now

        };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<Todo?> UpdateAsync(Guid todoId, Guid userId, string title, bool isCompleted)
    {
        var todo = await _context.Todos.FirstOrDefaultAsync(todo => todo.Id == todoId);
        if (todo == null || todo.UserId != userId)
        {
            return null;
        }

        todo.Title = title;
        todo.IsCompleted = isCompleted;
        todo.UpdatedAt = DateTime.Now;
        
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task<bool> DeleteAsync(Guid todoId, Guid userId)
    {
        var todo = await _context.Todos.FirstOrDefaultAsync(todo => todo.Id == todoId);
        if (todo == null || todo.UserId != userId)
        {
            return false;
        }

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Todo?> ToggleTodoAsync(Guid todoId, Guid userId)
    {
        var todo = await _context.Todos.FirstOrDefaultAsync(todo => todo.Id == todoId);
        if (todo == null || todo.UserId != userId)
        {
            return null;
        }

        todo.IsCompleted = !todo.IsCompleted;
        await _context.SaveChangesAsync();

        return todo;
    }
}