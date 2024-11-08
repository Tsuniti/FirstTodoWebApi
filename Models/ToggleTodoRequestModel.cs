namespace FirstTodoWebApi.Models;

public class ToggleTodoRequestModel
{
    public Guid TodoId { get; set; }
    public Guid UserId { get; set; }
}