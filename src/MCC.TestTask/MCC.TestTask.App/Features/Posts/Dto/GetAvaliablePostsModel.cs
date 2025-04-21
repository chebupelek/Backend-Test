namespace MCC.TestTask.App.Features.Posts.Dto;

public class GetAvaliablePostsModel 
{
    public List<Guid> Tags { get; set; } = new();

    public string? Author { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public PostSorting? Sorting { get; set; }

    public bool? OnlyMyCommunities { get; set; }

    public int Page { get; set; } = 1;

    public int Size { get; set; } = 5;
}