using System.ComponentModel.DataAnnotations;

namespace FirstTodoWebApi.Models;

public class CreateCommentRequestModel
{
    [Required(ErrorMessage = "Comment text is required to be filled")]
    public string Text { get; set; }
    
    public Guid TodoId { get; set; }
}