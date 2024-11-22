using FirstTodoWebApi.Entities;


namespace FirstTodoWebApi.Interfaces;

public interface ICommentService
{
    Task<Comment> CreateAsync(string text, Guid todoId, Guid userId);
    
    Task<bool> DeleteAsync(Guid commentId, Guid userId);

    Task<int?> CountByTodoIdAsync(Guid todoId, Guid userId);

    Task<IEnumerable<Comment>> GetRangeFromStartByTodoIdAsync(Guid todoId, int count, Guid userId);
}