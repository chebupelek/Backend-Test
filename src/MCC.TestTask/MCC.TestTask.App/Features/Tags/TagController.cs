using System.Net;
using MCC.TestTask.App.Features.Tags.Dto;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Features.Tags;

[Route("api/tag")]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;

    public TagController(TagService tagService)
    {
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
        return await _tagService.CreateTag(model.Name).ToActionResult();
    }
}