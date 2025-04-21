namespace MCC.TestTask.Domain;

public class Comment
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid CreatorId { get; set; }
    public User Creator { get; set; }

    public Guid PostId { get; set; }

    public Guid? ParentId { get; set; }
    public Comment? Parent { get; set; }

    public string Content { get; set; }

    public bool IsMarkedAsDeleted { get; set; }

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}