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
        var todos = await _todoService.GetAllAsync(UserId);
        return Ok(todos);
    }

    /// <summary>
    /// Get todo by id
    /// </summary>
    /// <param name="id">Todo id</param>
    /// <response code="200">Success</response>
    /// <response code="404">Todo with the specified ID was not found</response>
    /// <returns>Todo with the specified ID</returns>
    [ProducesResponseType(typeof(IQueryable<Todo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var todo = await _todoService.GetByIdAsync(id, UserId);
        
        if (todo is null)
            return NotFound("Todo with the specified ID was not found, or access denied");
        
        return Ok(todo);
    }
    
    
    /// <summary>
    /// Create new todo
    /// </summary>
    /// <param name="model">Model with title of todo</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <returns>Created todo</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.CreateAsync(model.Title, UserId);
        return Ok(todo);
    }

    
    /// <summary>
    /// Update todo
    /// </summary>
    /// <param name="model">Model with todo id, new title, new status(isCompleted)</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Todo not found or access denied</response>
    /// <returns>Updated todo</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.UpdateAsync(model.TodoId, UserId, model.Title, model.IsCompleted);

        if (todo is null)
            return NotFound("Todo not found or wrong user");

        return Ok(todo);
    }

    /// <summary>
    /// Delete todo
    /// </summary>
    /// <param name="model">Model with todo id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Todo not found or access denied</response>
    /// <returns>string</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _todoService.DeleteAsync(model.TodoId, UserId))
            return NotFound("Todo not found or wrong user");

        return Ok("Todo deleted");
    }
    /// <summary>
    /// Toggle todo status(IsCompeted)
    /// </summary>
    /// <param name="model">Model with todo id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Todo not found or access denied</response>
    /// <returns>string</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
    [HttpPut("toggle")]
    public async Task<IActionResult> ToggleTodo([FromBody] ToggleTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.ToggleTodoAsync(model.TodoId, UserId);
        if (todo is null)
            return NotFound("Todo not found or wrong user");
        
        return Ok(todo);
    }
    
}