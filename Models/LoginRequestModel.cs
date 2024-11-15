using System.ComponentModel.DataAnnotations;

namespace FirstTodoWebApi.Models;

public class LoginRequestModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}