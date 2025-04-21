using MCC.TestTask.App.Features.Comments.Dto;
using MCC.TestTask.Domain;
using MCC.TestTask.Persistance;
using MCC.TestTask.Infrastructure;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace MCC.TestTask.App.Features.Comments;

public class CommentService
{
    private readonly BlogDbContext _blogDbContext;

    public CommentService(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<Result<List<CommentDto>>> GetNestedCommentsAsync(Guid? userId, Guid commentId)
    {
        if (userId.HasValue && !await _blogDbContext.Users.AnyAsync(u => u.Id == userId))
            return CustomErrors.NotFound("User not found");

        if (!await _blogDbContext.Comments.AnyAsync(c => c.Id == commentId))
            return CustomErrors.NotFound("Comment not found");

        var communityType = await _blogDbContext.Communities
            .Where(c => c.Posts.Any(p => p.Comments.Any(x => x.Id == commentId)))
            .Select(c => c.CommunityType).FirstOrDefaultAsync();

        if (communityType == CommunityType.Private
            && (!userId.HasValue || !await _blogDbContext.Communities.AnyAsync(c =>
                (c.CreatorId == userId.Value
                 || c.Administrators.Any(a => a.Id == userId.Value)
                 || c.Subscribers.Any(s => s.Id == userId.Value))
                && c.Posts.Any(p => p.Comments.Any(x => x.Id == commentId)))))
            return CustomErrors.NotFound("Comment not found");

        return await _blogDbContext.Comments
            .Where(c => c.ParentId == commentId)
            .Select(c => c.ToDto())
            .ToListAsync();
    }

    public async Task<Result> CreateCommentAsync(Guid userId, Guid postId, CommentCreateModel model)
    {
        if (!await _blogDbContext.Users.AnyAsync(u => u.Id == userId))
            return CustomErrors.NotFound("User not found");

        if (!await _blogDbContext.Posts.AnyAsync(c => c.Id == postId))
            return CustomErrors.NotFound("Post not found");

        if (model.ParentId.HasValue &&
            !await _blogDbContext.Posts.AnyAsync(p =>
                p.Id == postId
                && p.Comments.Any(c => c.Id == model.ParentId.Value && !c.IsMarkedAsDeleted)))
            return CustomErrors.NotFound("Comment not found");

        var communityType = await _blogDbContext.Communities
            .Where(c => c.Posts.Any(p => p.Id == postId))
            .Select(c => c.CommunityType).FirstOrDefaultAsync();

        if (communityType == CommunityType.Private
            && !await _blogDbContext.Communities.AnyAsync(c =>
                (c.CreatorId == userId
                 || c.Administrators.Any(a => a.Id == userId)
                 || c.Subscribers.Any(s => s.Id == userId))
                && c.Posts.Any(p => p.Id == postId)))
            return CustomErrors.NotFound("Post not found");

        var comment = new Comment
        {
            CreatorId = userId,
            PostId = postId,
            CreatedAt = DateTime.UtcNow,
            ParentId = model.ParentId,
            Content = model.Content
        };

        _blogDbContext.Comments.Add(comment);
        await _blogDbContext.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> EditCommentAsync(Guid userId, Guid commentId, string content)
    {
        if (!await _blogDbContext.Users.AnyAsync(u => u.Id == userId))
            return CustomErrors.NotFound("User not found");

        var communityType = await _blogDbContext.Communities
            .Where(c => c.Posts.Any(p => p.Comments.Any(x => x.Id == commentId)))
            .Select(c => c.CommunityType).FirstOrDefaultAsync();

        // forbid editing comments after leaving closed community
        if (communityType == CommunityType.Private
            && !await _blogDbContext.Communities.AnyAsync(c =>
                (c.CreatorId == userId
                 || c.Administrators.Any(a => a.Id == userId)
                 || c.Subscribers.Any(s => s.Id == userId))
                && c.Posts.Any(p => p.Comments.Any(x => x.Id == commentId))))
            return CustomErrors.NotFound("Comment not found");

        var comment = await _blogDbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId && !c.IsMarkedAsDeleted);

        if (comment == null)
            return CustomErrors.NotFound("Comment not found");

        if (comment.CreatorId != userId)
            return CustomErrors.Forbidden("User is not the author of the comment");

        comment.Content = content;
        comment.ModifiedAt = DateTime.UtcNow;
        _blogDbContext.Update(comment);
        await _blogDbContext.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteCommentAsync(Guid userId, Guid commentId)
    {
        if (!await _blogDbContext.Users.AnyAsync(u => u.Id == userId))
            return CustomErrors.NotFound("User not found");

        var communityType = await _blogDbContext.Communities
            .Where(c => c.Posts.Any(p => p.Comments.Any(x => x.Id == commentId)))
            .Select(c => c.CommunityType).FirstOrDefaultAsync();

        // forbid deleting comments after leaving closed community
        if (communityType == CommunityType.Private
            && !await _blogDbContext.Communities.AnyAsync(c =>
                (c.CreatorId == userId
                 || c.Administrators.Any(a => a.Id == userId)
                 || c.Subscribers.Any(s => s.Id == userId))
                && c.Posts.Any(p => p.Comments.Any(x => x.Id == commentId))))
            return CustomErrors.NotFound("Comment not found");

        var comment = await _blogDbContext.Comments
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == commentId && !c.IsMarkedAsDeleted);

        if (comment == null)
            return CustomErrors.NotFound("Comment not found");

        if (comment.CreatorId != userId)
            return CustomErrors.Forbidden("User is not the author of the comment");

        if (comment.Replies.Any())
        {
            comment.IsMarkedAsDeleted = true;
            _blogDbContext.Update(comment);
        }
        else
        {
            _blogDbContext.Remove(comment);
        }

        await _blogDbContext.SaveChangesAsync();

        return Result.Ok();
    }
}