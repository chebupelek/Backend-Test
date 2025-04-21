using FluentResults;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MCC.TestTask.App.Services.Mail;

public class MailingService
{
    private readonly MailingServiceOptions _options;
    private readonly ISmtpClient _smtpClient;

    public MailingService(ISmtpClient smtpClient, IOptions<MailingServiceOptions> options)
    {
        _smtpClient = smtpClient;
        _options = options.Value;
    }

    public async Task<Result> SendMail(string toName, string toEmail, string subject, string body)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        message.To.Add(
            new MailboxAddress(toName, toEmail)
        );

        message.Subject = subject;

        message.Body = new TextPart("plain") { Text = body };

        try
        {
            await _smtpClient.ConnectAsync(_options.Host, _options.Port);

            if (
                !string.IsNullOrEmpty(_options.Username)
                && !string.IsNullOrEmpty(_options.Password)
            )
                await _smtpClient.AuthenticateAsync(_options.Username, _options.Password);

            await _smtpClient.SendAsync(message);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        finally
        {
            await _smtpClient.DisconnectAsync(true);
        }

        return Result.Ok();
    }
}