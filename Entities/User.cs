using System.Text.Json.Serialization;

namespace FirstTodoWebApi.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    [JsonIgnore]
    public ICollection<Todo> Todos { get; set; }
}