using MCC.TestTask.App.Utils.Pagination;

namespace MCC.TestTask.App.Features.Posts.Dto;

public class PostPagedListDto
{
    public List<PostDto> Posts { get; set; }

    public PaginationDto Pagination { get; set; }
}