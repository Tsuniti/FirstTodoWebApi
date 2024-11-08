namespace FirstTodoWebApi.Entities;

public class Todo : BaseEntity
{

    public Guid UserId { get; set; }
    
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    
}