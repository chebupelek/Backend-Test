using System.Net;
using MCC.TestTask.App.Features.Tags.Dto;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MCC.TestTask.App.Features.Posts;
using MCC.TestTask.App.Services.Auth;

namespace MCC.TestTask.App.Features.Tags;

[Route("api/tag")]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;
    private readonly UserAccessor _userAccessor;

    public TagController(UserAccessor userAccessor, TagService tagService)
    {
        _userAccessor = userAccessor;
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TagDto>>> GetTags()
    {
        return await _tagService.GetAllTagsAsync().ToActionResult();
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Guid>> CreateTag([FromBody] CreateTagModel model)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _tagService.CreateTag(userId, model.Name))
            .ToActionResult();
    }
}