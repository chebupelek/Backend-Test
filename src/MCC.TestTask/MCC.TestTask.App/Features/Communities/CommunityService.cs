using MCC.TestTask.App.Features.Communities.Dto;
using MCC.TestTask.Domain;
using MCC.TestTask.Persistance;
using MCC.TestTask.Infrastructure;
using FluentResults;
using FluentResults.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MCC.TestTask.App.Features.Communities;

public class CommunityService
{
    private readonly BlogDbContext _blogDbContext;

    public CommunityService(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<Result<Guid>> CreateCommunityAsync(Guid userId, CommunityCreateModel model)
    {
        return await CheckUserExistsAsync(userId).Bind(async Task<Result<Guid>> () =>
        {
            if (_blogDbContext.Communities.Any(c => c.Name == model.Name))
                return CustomErrors.ValidationError($"Community with name {model.Name} already exists.");

            var community = new Community
            {
                Name = model.Name,
                Description = model.Description,
                CommunityType = model.CommunityType,
                CreatedAt = DateTime.UtcNow,
                CreatorId = userId
            };

            _blogDbContext.Communities.Add(community);
            await _blogDbContext.SaveChangesAsync();
            return community.Id;
        });
    }

    public async Task<Result<List<CommunityDto>>> GetAllCommunitiesAsync()
    {
        return Result.Ok(await _blogDbContext.Communities.Select(c => c.ToDto()).ToListAsync());
    }

    public async Task<Result<List<CommunityUserDto>>> GetAllCommunitiesForUserIdAsync(Guid userId)
    {
        return await CheckUserExistsAsync(userId).Bind<List<CommunityUserDto>>(async () =>
            await _blogDbContext.Communities
                .Where(c => c.Subscribers.Any(u => u.Id == userId))
                .Select(c => new CommunityUserDto
                    { CommunityId = c.Id, UserId = userId, Role = CommunityUserRole.Subscriber })
                .Concat(_blogDbContext.Communities
                    .Where(c => c.Administrators.Any(a => a.Id == userId) || c.CreatorId == userId)
                    .Select(c => new CommunityUserDto
                        { CommunityId = c.Id, UserId = userId, Role = CommunityUserRole.Administrator }))
                .ToListAsync());
    }

    public async Task<Result<CommunityFullDto?>> GetCommunityFullAsync(Guid communityId)
    {
        return await CheckCommunityExistsAsync(communityId).Bind(async Task<Result<CommunityFullDto?>> () =>
            await _blogDbContext.Communities
                .Select(c => c.ToFullDto())
                .FirstOrDefaultAsync(c => c.Id == communityId));
    }

    public async Task<Result<CommunityUserRole?>> GetCommunityUserRoleAsync(Guid communityId, Guid userId)
    {
        var communityExists = await _blogDbContext.Communities
            .AnyAsync(c => c.Id == communityId);
        var userExists = await _blogDbContext.Users.AnyAsync(u => u.Id == userId);

        if (!communityExists ||
            !userExists)
            return CustomErrors.NotFound("Non-existent user, community or tag");

        return await _blogDbContext.Communities
            .Where(p => p.Id == communityId)
            .Select(p =>
                p.CreatorId == userId || p.Administrators.Any(a => a.Id == userId)
                    ? CommunityUserRole.Administrator
                    : p.Subscribers.Any(s => s.Id == userId)
                        ? (CommunityUserRole?)CommunityUserRole.Subscriber
                        : null).FirstOrDefaultAsync();
    }

    public async Task<Result> UpdateCommunityAdminsList(Guid userId, Guid communityId, List<Guid> adminIds)
    {
        return await CheckUserExistsAsync(userId)
            .Bind(async () => await CheckCommunityExistsAsync(communityId))
            .Bind(async () => Result.FailIf(
                await _blogDbContext.Communities.AnyAsync(c => c.Id == communityId && c.CreatorId != userId),
                new ForbiddenError("User is not authorized to update community admins.")))
            .Bind(async () =>
            {
                var newAdmins = await _blogDbContext.Users.Where(u => adminIds.Contains(u.Id)).ToListAsync();

                if (newAdmins.Count != adminIds.Distinct().Count())
                    return CustomErrors.ValidationError("Invalid admin id");

                var community = _blogDbContext.Communities.First(c => c.Id == communityId);
                _blogDbContext.Entry(community).Collection(c => c.Administrators).CurrentValue = newAdmins;
                await _blogDbContext.SaveChangesAsync();

                return Result.Ok();
            });
    }

    public async Task<Result> SubscribeUserToCommunityAsync(Guid communityId, Guid userId)
    {
        var community = await _blogDbContext.Communities
            .FirstOrDefaultAsync(c => c.Id == communityId);
        var user = await _blogDbContext.Users.Include(u => u.SubscribedTo).FirstOrDefaultAsync(u => u.Id == userId);

        if (community is null ||
            user is null)
            return CustomErrors.NotFound("Non-existent user or community");

        if (user.SubscribedTo.Any(p => p.Id == community.Id))
            return CustomErrors.ValidationError("User already subscribed to this community");

        user.SubscribedTo.Add(community);
        await _blogDbContext.SaveChangesAsync();

        return Result.Ok();
    }


    public async Task<Result> UnsubscribeUserFromCommunityAsync(Guid communityId, Guid userId)
    {
        var community = await _blogDbContext.Communities
            .FirstOrDefaultAsync(c => c.Id == communityId);
        var user = await _blogDbContext.Users.Include(u => u.SubscribedTo).FirstOrDefaultAsync(u => u.Id == userId);

        if (community is null ||
            user is null)
            return CustomErrors.NotFound("Non-existent user, community or tag");

        if (user.SubscribedTo.All(p => p.Id != community.Id))
            return CustomErrors.ValidationError("User already not subscribed to this community");

        user.SubscribedTo.Remove(community);
        await _blogDbContext.SaveChangesAsync();

        return Result.Ok();
    }

    private async Task<Result> CheckCommunityExistsAsync(Guid communityId)
    {
        return await _blogDbContext.Communities.AnyAsync(c => c.Id == communityId)
            ? Result.Ok()
            : CustomErrors.NotFound("Non-existent community");
    }

    private async Task<Result> CheckUserExistsAsync(Guid userId)
    {
        return await _blogDbContext.Users.AnyAsync(u => u.Id == userId)
            ? Result.Ok()
            : CustomErrors.NotFound("Non-existent user");
    }
}