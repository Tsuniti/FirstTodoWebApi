using FirstTodoWebApi.Interfaces;
using FirstTodoWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstTodoWebApi.Controllers;
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtGenerator _jwtGenerator;

    public AuthController(IUserService userService, IJwtGenerator jwtGenerator)
    {
        _userService = userService;
        _jwtGenerator = jwtGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        // Если модель не валидная - возвращаем ошибку со статус кодом 400
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Проверяем, есть ли пользователь с таким логином
        bool usernameExists = await _userService.UsernameExistsAsync(model.Username);

        // Если такое уже есть - ошибка
        if (usernameExists)
            return Conflict(new { error = "Username already exists" });

        await _userService.CreateUserAsync(model.Username, model.Password);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        // Если модель не валидная - возвращаем ошибку со статус кодом 400
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Ищем айди пользователя по логину и паролю
        Guid? userId = await _userService.GetUserIdByCredentialsAsync(model.Username, model.Password);

        if (userId is null)
            return NotFound(new { error = "User not found" });

        // jwt токен
        var token = _jwtGenerator.GenerateToken((Guid)userId);

        return Ok(new { token });
    }
}