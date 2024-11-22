using System.Text.Json.Serialization;

namespace FirstTodoWebApi.Entities;

public class Comment : BaseEntity
{
    public string Text { get; set; }
    public Guid TodoId { get; set; }
    [JsonIgnore]
    public Todo Todo { get; set; }
}