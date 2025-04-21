namespace MCC.TestTask.App.Services.Mail;

public class MailingServiceOptions
{
    public string Host { get; set; }

    public int Port { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string FromName { get; set; }

    public string FromAddress { get; set; }
}