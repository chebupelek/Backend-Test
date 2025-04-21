namespace MCC.TestTask.Domain.Extensions;

public static class PostExtensions
{
    public static IEnumerable<Post> ReadableByUser(this IEnumerable<Post> query, Guid? userId)
    {
        return query.Where(p => p.Community == null
                                || p.Community.CommunityType == CommunityType.Public
                                || (userId.HasValue && p.Community.CommunityType == CommunityType.Private
                                                    && (p.Community.CreatorId == userId.Value
                                                        || p.Community.Administrators.Any(a => a.Id == userId.Value)
                                                        || p.Community.Subscribers.Any(s => s.Id == userId.Value))));
    }
    
    public static IQueryable<Post> ReadableByUser(this IQueryable<Post> query, Guid? userId)
    {
        return query.Where(p => p.Community == null
                                || p.Community.CommunityType == CommunityType.Public
                                || (userId.HasValue && p.Community.CommunityType == CommunityType.Private
                                                    && (p.Community.CreatorId == userId.Value
                                                        || p.Community.Administrators.Any(a => a.Id == userId.Value)
                                                        || p.Community.Subscribers.Any(s => s.Id == userId.Value))));
    }
}