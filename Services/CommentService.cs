using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstTodoWebApi.Services;

public class CommentService : ICommentService
{
    
    private readonly IApplicationDbContext _context;

    public CommentService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Comment> CreateAsync( string text, Guid todoId, Guid userId)
    {

        var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == todoId);
        if (todo is null || todo.UserId != userId)
            return null; // access denied or todo not found
        
        
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            TodoId = todoId,
            Text = text,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<bool> DeleteAsync(Guid commentId, Guid userId)
    {
        var comment = await _context.Comments
            .Include(c => c.Todo)
            .FirstOrDefaultAsync(c => c.Id == commentId);
        
        if (comment is null || comment.Todo.UserId != userId)
            return false; // comment not found or access denied

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int?> CountByTodoIdAsync(Guid todoId, Guid userId)
    {
        var todo = await _context.Todos
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == todoId);
        
        if (todo is null || todo.UserId != userId)
            return null; // access denied or todo not found

        return todo.Comments.Count;
    }

    public async Task<IEnumerable<Comment>> GetRangeFromStartByTodoIdAsync(Guid todoId, int count, Guid userId)
    {
        var todo = await _context.Todos
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == todoId);
        
        if (todo is null || todo.UserId != userId)
            return null; // access denied or todo not found

        return todo.Comments.Take(count);
    }
}