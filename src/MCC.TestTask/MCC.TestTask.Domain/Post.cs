namespace MCC.TestTask.Domain;

public class Post
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CommunityId { get; set; }
    public Community? Community { get; set; }

    public int ReadingTime { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<User> LikedBy { get; set; } = new List<User>();

    public Guid AuthorId { get; set; }
    public User Author { get; set; }

    public Guid? AddressObjectId { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}