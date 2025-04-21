namespace MCC.TestTask.App.Features.Posts.Dto;

public class PostListFilter
{
    public Guid? CommunityId { get; set; }

    public string? Author { get; set; }

    public List<Guid> TagIds { get; set; } = new();

    public int? MinReadingTime { get; set; }

    public int? MaxReadingTime { get; set; }

    public bool? OnlyMyCommunities { get; set; }
}