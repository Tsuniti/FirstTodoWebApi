using System.Text.Json.Serialization;

namespace FirstTodoWebApi.Entities;

public class Todo : BaseEntity  
{

    public Guid UserId { get; set; }
    
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
}