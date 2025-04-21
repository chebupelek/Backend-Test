namespace MCC.TestTask.Domain;

public class User
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string FullName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<Community> SubscribedTo { get; set; } = new List<Community>();
}