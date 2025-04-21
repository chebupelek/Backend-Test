using System.Linq.Expressions;
using MCC.TestTask.Domain;
using NeinLinq;

namespace MCC.TestTask.App.Features.Tags.Dto;

public static class TagDtoConverters
{
    private static readonly Lazy<Func<Tag, TagDto>> ToDtoCompiled
        = new(() => ToDto().Compile());

    [InjectLambda]
    public static TagDto ToDto(this Tag tag)
    {
        return ToDtoCompiled.Value(tag);
    }

    public static Expression<Func<Tag, TagDto>> ToDto()
    {
        return tag => new TagDto
        {
            Id = tag.Id,
            CreateTime = tag.CreateTime,
            Name = tag.Name
        };
    }
}