using System.Linq.Expressions;
using MCC.TestTask.App.Features.Users.Dto;
using MCC.TestTask.Domain;
using NeinLinq;

namespace MCC.TestTask.App.Features.Communities.Dto;

public static class CommunityDtoConverters
{
    private static readonly Lazy<Func<Community, CommunityDto>> ToDtoCompiled = new(ToDto().Compile());

    private static readonly Lazy<Func<Community, CommunityFullDto>> ToFullDtoCompiled = new(ToFullDto().Compile());

    [InjectLambda]
    public static CommunityDto ToDto(this Community community)
    {
        return ToDtoCompiled.Value(community);
    }

    public static Expression<Func<Community, CommunityDto>> ToDto()
    {
        return community => new CommunityDto
        {
            Id = community.Id,
            CreateTime = community.CreatedAt,
            Name = community.Name,
            Description = community.Description,
            IsClosed = community.CommunityType == CommunityType.Private,
            SubscribersCount = community.Subscribers.Count + community.Administrators.Count + 1,
        };
    }

    [InjectLambda]
    public static CommunityFullDto ToFullDto(this Community community)
    {
        return ToFullDtoCompiled.Value(community);
    }

    public static Expression<Func<Community, CommunityFullDto>> ToFullDto()
    {
        return community => new CommunityFullDto
        {
            Id = community.Id,
            CreateTime = community.CreatedAt,
            Name = community.Name,
            Description = community.Description,
            IsClosed = community.CommunityType == CommunityType.Private,
            SubscribersCount = community.Subscribers.Count + community.Administrators.Count + 1,
            Administrators = community.Administrators.Select(a => a.ToDto()).ToList(),
            Creator = community.Creator.ToDto(),
        };
    }
}