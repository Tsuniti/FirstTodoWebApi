using System.Security.Claims;
using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using FirstTodoWebApi.Models;
using FirstTodoWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FirstTodoWebApi.Controllers;


[Route("comment")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    
    /// <summary>
    /// Create new comment
    /// </summary>
    /// <param name="model">Model with text of comment and todo id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <returns>Created comment</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCommentRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentService.CreateAsync(model.Text, model.TodoId, UserId);
        return Ok(comment);
    }
    
    /// <summary>
    /// Delete comment
    /// </summary>
    /// <param name="model">Model with comment id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Comment not found or access denied</response>
    /// <returns>string</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteCommentRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!await _commentService.DeleteAsync(model.CommentId, UserId))
            return NotFound("Comment not found or wrong user");

        return Ok("Comment deleted");
    }

    /// <summary>
    /// Get the number of comments for a todo
    /// </summary>
    /// <param name="id">Todo id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Comment not found or access denied</response>
    /// <returns>The number of comments for a todo</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
    [HttpGet("{todoId:guid}")]
    public async Task<IActionResult> CountByTodoIdAsync(Guid todoId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var count = _commentService.CountByTodoIdAsync(todoId, UserId);
        
        if (count is null)
            return NotFound("Comment not found or access denied");
        
        return Ok(count);
    }

    /// <summary>
    /// Get range from start by todo id
    /// </summary>
    /// <param name="id">Todo id</param>
    /// <param name="count">Desired number of comments</param>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Comment not found or access denied</response>
    /// <returns>List of comments</returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
    [HttpGet("{todoId:guid}/{count:int}")]
    public async Task<IActionResult> GetRangeFromStartByTodoIdAsync(Guid todoId, int count)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (count <= 0)
            return BadRequest("Count value must be greater than 0");

        var comments = _commentService.GetRangeFromStartByTodoIdAsync(todoId, count, UserId);

        if (comments is null)
            return NotFound("Access denied or todo not found");
        
        return Ok(comments);
    }
}