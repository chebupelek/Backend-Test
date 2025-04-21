namespace MCC.TestTask.Domain.Extensions;

public static class CommunityExtensions
{
    public static IQueryable<Community> ReadableByUser(this IQueryable<Community> communities, Guid userId)
    {
        return communities.Where(c =>
            c.CommunityType == CommunityType.Public
            || (c.CommunityType == CommunityType.Private
                && (c.CreatorId == userId
                    || c.Administrators.Any(a => a.Id == userId)
                    || c.Subscribers.Any(s => s.Id == userId))));
    }
}