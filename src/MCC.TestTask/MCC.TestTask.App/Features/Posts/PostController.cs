using System.Net;
using MCC.TestTask.App.Features.Posts.Dto;
using MCC.TestTask.App.Services.Auth;
using MCC.TestTask.App.Utils.Pagination;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Features.Posts;

[Route("api/post")]
public class PostController : Controller
{
    private readonly PostService _postService;
    private readonly UserAccessor _userAccessor;

    public PostController(UserAccessor userAccessor, PostService postService)
    {
        _userAccessor = userAccessor;
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<PostPagedListDto>> Get([FromQuery] GetAvaliablePostsModel model)
    {
        var userIdResult = _userAccessor.GetUserId();
        return await _postService.GetAllPostsAsync(userIdResult.IsSuccess ? userIdResult.Value : null,
                new PostListFilter
                {
                    TagIds = model.Tags,
                    Author = model.Author,
                    MinReadingTime = model.Min,
                    MaxReadingTime = model.Max,
                    OnlyMyCommunities = model.OnlyMyCommunities
                },
                model.Sorting,
                new PaginationModel { Page = model.Page, Size = model.Size })
            .ToActionResult();
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Guid>> CreatePersonalPostAsync([FromBody] CreatePostModel model)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _postService.CreatePostAsync(userId, null, model))
            .ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostFullDto>> GetFullPost(Guid id)
    {
        var userIdResult = _userAccessor.GetUserId();
        return await _postService.GetPostAsync(id, userIdResult.IsSuccess ? userIdResult.Value : null)
            .ToActionResult();
    }


    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<ActionResult> LikePostAsync([FromRoute] Guid id)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _postService.LikePostAsync(id, userId))
            .ToActionResult();
    }

    [Authorize]
    [HttpDelete("{id}/like")]
    public async Task<ActionResult> DislikePostAsync([FromRoute] Guid id)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _postService.DislikePostAsync(id, userId))
            .ToActionResult();
    }
}