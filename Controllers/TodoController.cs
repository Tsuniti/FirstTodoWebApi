using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using FirstTodoWebApi.Models;
using FirstTodoWebApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FirstTodoWebApi.Controllers;

[Route("todo")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;


    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todos = await _todoService.GetAllAsync();
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

        var todo = await _todoService.CreateAsync(model.Title, Guid.Empty);
        return Ok(todo);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.UpdateAsync(model.TodoId, model.UserId, model.Title, model.IsCompleted);

        if (todo is null)
            return BadRequest("Todo not found or wrong user");

        return Ok(todo);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _todoService.DeleteAsync(model.TodoId, model.UserId))
            return BadRequest("Todo not found or wrong user");

        return Ok("Todo deleted");
    }

    [HttpPut("toggle")]
    public async Task<IActionResult> ToggleTodo([FromBody] ToggleTodoRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var todo = await _todoService.ToggleTodoAsync(model.TodoId, model.UserId);
        if (todo is null)
            return BadRequest("Todo not found or wrong user");
        
        return Ok(todo);

    }
    
}