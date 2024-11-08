using System.ComponentModel.DataAnnotations;

namespace FirstTodoWebApi.Models;

public class CreateTodoRequestModel
{
    [Required(ErrorMessage = "Title is required to be filled")]
    [MinLength(3, ErrorMessage = "Title is too short")]
    
    public string Title { get; set; }
}