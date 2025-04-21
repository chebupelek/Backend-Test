using System.Linq.Expressions;
using MCC.TestTask.Domain;
using NeinLinq;

namespace MCC.TestTask.App.Features.Comments.Dto;

public static class CommentDtoConverters
{
    private static readonly Lazy<Func<Comment, CommentDto>> ToDtoCompiled
        = new(() => ToDto().Compile());

    [InjectLambda]
    public static CommentDto ToDto(this Comment comment)
    {
        return ToDtoCompiled.Value(comment);
    }

    public static Expression<Func<Comment, CommentDto>> ToDto()
    {
        return comment => new CommentDto
        {
            Id = comment.Id,
            AuthorId = comment.IsMarkedAsDeleted ? null : comment.CreatorId,
            Author = comment.IsMarkedAsDeleted ? null : comment.Creator.FullName,
            Content = comment.IsMarkedAsDeleted ? null : comment.Content,
            CreateTime = comment.CreatedAt,
            ModifiedDate = comment.ModifiedAt,
            SubComments = comment.Replies.Count
        };
    }
}