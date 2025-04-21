namespace MCC.TestTask.Domain;

public class Tag
{
    public Guid Id { get; set; }

    public DateTime CreateTime { get; set; }

    public string Name { get; set; }

    public Guid CreatorId { get; init; }
    public User Creator { get; set; }
}