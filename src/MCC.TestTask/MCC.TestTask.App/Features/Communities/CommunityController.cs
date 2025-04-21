using System.Net;
using MCC.TestTask.App.Features.Communities.Dto;
using MCC.TestTask.App.Features.Posts;
using MCC.TestTask.App.Features.Posts.Dto;
using MCC.TestTask.App.Services.Auth;
using MCC.TestTask.App.Utils.Pagination;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Features.Communities;

[ApiController]
[Route("api/community")]
public class CommunityController : ControllerBase
{
    private readonly CommunityService _communityService;
    private readonly PostService _postService;
    private readonly UserAccessor _userAccessor;

    public CommunityController(CommunityService communityService, PostService postService, UserAccessor userAccessor)
    {
        _communityService = communityService;
        _postService = postService;
        _userAccessor = userAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<List<CommunityDto>>> GetAllCommunitiesAsync()
    {
        return await _communityService.GetAllCommunitiesAsync()
            .ToActionResult();
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<List<CommunityUserDto>>> GetUserCommunitiesAsync()
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _communityService.GetAllCommunitiesForUserIdAsync(userId))
            .ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommunityFullDto>> GetCommunityByIdAsync([FromRoute] Guid id)
    {
        return await _communityService.GetCommunityFullAsync(id).ToActionResult();
    }

    [HttpGet("{id}/post")]
    public async Task<ActionResult<List<PostDto>>> GetAllPostsAsync([FromRoute] Guid id,
        [FromQuery] GetCommunityPostsModel model)
    {
        var userIdResult = _userAccessor.GetUserId();
        return await _postService.GetAllPostsAsync(
            userIdResult.IsSuccess ? userIdResult.Value : null,
            new PostListFilter { CommunityId = id, TagIds = model.Tags },
            model.Sorting,
            new PaginationModel { Page = model.Page, Size = model.Size }).ToActionResult();
    }

    [Authorize]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [HttpPost("{id}/post")]
    public async Task<ActionResult> CreatePostAsync([FromRoute] Guid id, [FromBody] CreatePostModel model)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _postService.CreatePostAsync(userId, id, model))
            .ToActionResult();
    }

    [Authorize]
    [HttpGet("{id}/role")]
    public async Task<ActionResult<CommunityUserRole>> GetRoleAsync([FromRoute] Guid id)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _communityService.GetCommunityUserRoleAsync(id, userId))
            .ToActionResult();
    }

    [Authorize]
    [HttpPost("{id}/subscribe")]
    public async Task<ActionResult> SubscribeToCommunityAsync([FromRoute] Guid id)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _communityService.SubscribeUserToCommunityAsync(id, userId))
            .ToActionResult();
    }

    [Authorize]
    [HttpDelete("{id}/unsubscribe")]
    public async Task<ActionResult> UnsubscribeToCommunityAsync([FromRoute] Guid id)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _communityService.UnsubscribeUserFromCommunityAsync(id, userId))
            .ToActionResult();
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Guid>> CreateCommunityAsync([FromBody] CommunityCreateModel model)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _communityService.CreateCommunityAsync(userId, model))
            .ToActionResult();
    }

    [Authorize]
    [HttpPut("{id}/admins")]
    public async Task<ActionResult<Guid>> EditCommunityAdminsAsync([FromRoute] Guid id,
        [FromBody] List<Guid> adminIds)
    {
        return await _userAccessor.GetUserId()
            .Bind(userId => _communityService.UpdateCommunityAdminsList(userId, id, adminIds))
            .ToActionResult();
    }
}