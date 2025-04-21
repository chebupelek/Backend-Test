using System.Net;
using MCC.TestTask.App.Features.Comments.Dto;
using MCC.TestTask.App.Services.Auth;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Features.Comments;

[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;
    private readonly UserAccessor _userAccessor;

    public CommentController(CommentService commentService, UserAccessor userAccessor)
    {
        _commentService = commentService;
        _userAccessor = userAccessor;
    }

    [HttpGet("{id}/tree")]
    public async Task<ActionResult<List<CommentDto>>> GetTree(Guid id)
    {
        var userIdResult = _userAccessor.GetUserId();
        return await _commentService.GetNestedCommentsAsync(userIdResult.IsSuccess ? userIdResult.Value : null, id)
            .ToActionResult();
    }

    [Authorize]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [HttpPost("/api/post/{id}/comment")]
    public async Task<ActionResult<CommentDto>> CreateComment(Guid id, [FromBody] CommentCreateModel model)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _commentService.CreateCommentAsync(userId, id, model))
            .ToActionResult();
    }

    [Authorize]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [HttpPut("{id}")]
    public async Task<ActionResult> EditComment(Guid id, [FromBody] CommentEditModel model)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _commentService.EditCommentAsync(userId, id, model.Content))
            .ToActionResult();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(Guid id)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _commentService.DeleteCommentAsync(userId, id))
            .ToActionResult();
    }
}