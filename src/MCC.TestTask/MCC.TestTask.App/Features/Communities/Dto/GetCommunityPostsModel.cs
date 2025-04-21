using MCC.TestTask.App.Features.Posts.Dto;

namespace MCC.TestTask.App.Features.Communities.Dto;

public class GetCommunityPostsModel
{
    public List<Guid> Tags { get; set; } = new();

    public PostSorting? Sorting { get; set; }

    public int Page { get; set; } = 1;

    public int Size { get; set; } = 5;
}