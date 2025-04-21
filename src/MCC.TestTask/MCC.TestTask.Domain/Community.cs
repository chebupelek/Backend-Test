namespace MCC.TestTask.Domain;

public class Community
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public Guid CreatorId { get; init; }
    public User Creator { get; set; }

    public IList<User> Administrators { get; init; }

    public IList<User> Subscribers { get; init; }

    public IList<Post> Posts { get; init; }

    public CommunityType CommunityType { get; init; }
}