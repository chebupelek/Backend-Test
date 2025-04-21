namespace MCC.TestTask.App.Features.Comments.Dto;

public class CommentDto
{
    public Guid Id { get; set; }

    public DateTime CreateTime { get; set; }

    public string? Content { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public Guid? AuthorId { get; set; }
    public string? Author { get; set; }

    public int SubComments { get; set; }
}