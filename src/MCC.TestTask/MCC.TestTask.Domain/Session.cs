namespace MCC.TestTask.Domain;

public class Session
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string LastIp { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAfter { get; set; }
}