using System.Linq.Expressions;
using MCC.TestTask.App.Features.Comments.Dto;
using MCC.TestTask.App.Features.Tags.Dto;
using MCC.TestTask.Domain;
using NeinLinq;

namespace MCC.TestTask.App.Features.Posts.Dto;

public static class PostDtoConverters
{
    private static readonly Lazy<Func<Post, Guid?, PostDto>> ToDtoCompiled
        = new(() => ToDto().Compile());

    private static readonly Lazy<Func<Post, Guid?, PostFullDto>> ToFullDtoCompiled
        = new(() => ToFullDto().Compile());

    [InjectLambda]
    public static PostDto ToDto(this Post post, Guid? userId)
    {
        return ToDtoCompiled.Value(post, userId);
    }

    public static Expression<Func<Post, Guid?, PostDto>> ToDto()
    {
        return (post, userId) => new PostDto
        {
            Id = post.Id,
            CreateTime = post.CreatedAt,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.ImageUrl,
            AuthorId = post.AuthorId,
            Author = post.Author.FullName,
            CommunityId = post.CommunityId,
            CommunityName = post.Community != null ? post.Community.Name : null,
            Likes = post.LikedBy.Count,
            HasLike = userId != null && post.LikedBy.Any(lb => lb.Id == userId),
            CommentsCount = post.Comments.Count,
            Tags = post.Tags.Select(t => t.ToDto()).ToList()
        };
    }

    [InjectLambda]
    public static PostFullDto ToFullDto(this Post post, Guid? userId)
    {
        return ToFullDtoCompiled.Value(post, userId);
    }

    public static Expression<Func<Post, Guid?, PostFullDto>> ToFullDto()
    {
        return (post, userId) => new PostFullDto
        {
            Id = post.Id,
            CreateTime = post.CreatedAt,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.ImageUrl,
            AuthorId = post.AuthorId,
            Author = post.Author.FullName,
            CommunityId = post.CommunityId,
            CommunityName = post.Community != null ? post.Community.Name : null,
            Likes = post.LikedBy.Count,
            HasLike = userId != null && post.LikedBy.Any(lb => lb.Id == userId),
            CommentsCount = post.Comments.Count,
            Tags = post.Tags.Select(t => t.ToDto()).ToList(),
            Comments = post.Comments.Where(c => !c.ParentId.HasValue).Select(c => c.ToDto()).ToList()
        };
    }
}