using System.Security.Claims;
using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using FirstTodoWebApi.Models;
using FirstTodoWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FirstTodoWebApi.Controllers;

[Route("todo")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>
    /// Get all todos
    /// </summary>
    /// <response code="200">Success</response>
    /// <returns>List of todos</returns>
    [ProducesResponseType(typeof(IQueryable<Todo>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Console.WriteLine("Userid: " + UserId);
        var todos = await _todoService.GetAllAsync(UserId);
        return Ok(todos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _todoService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.CreateAsync(model.Title, UserId);
        return Ok(todo);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.UpdateAsync(model.TodoId, UserId, model.Title, model.IsCompleted);

        if (todo is null)
            return BadRequest("Todo not found or wrong user");

        return Ok(todo);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _todoService.DeleteAsync(model.TodoId, UserId))
            return BadRequest("Todo not found or wrong user");

        return Ok("Todo deleted");
    }

    [HttpPut("toggle")]
    public async Task<IActionResult> ToggleTodo([FromBody] ToggleTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.ToggleTodoAsync(model.TodoId, UserId);
        if (todo is null)
            return BadRequest("Todo not found or wrong user");
        
        return Ok(todo);

    }
    
}