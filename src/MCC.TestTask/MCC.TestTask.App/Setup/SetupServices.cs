using MCC.TestTask.App.Features.Authors;
using MCC.TestTask.App.Features.Comments;
using MCC.TestTask.App.Features.Communities;
using MCC.TestTask.App.Features.Posts;
using MCC.TestTask.App.Features.Tags;
using MCC.TestTask.App.Features.Users;
using MCC.TestTask.App.Services.Auth;
using MCC.TestTask.App.Services.Mail;
using MCC.TestTask.Domain;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;

namespace MCC.TestTask.App.Setup;

public static class SetupServices
{
    public static void AddServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.AddScoped<PasswordHasher<User>>(provider => new PasswordHasher<User>());
        services.AddScoped<TokenService>();
        services.AddSingleton<SessionService>();
        services.AddScoped<UserService>();
        services.AddScoped<UserAccessor>();
        services.AddScoped<CommunityService>();
        services.AddScoped<PostService>();
        services.AddScoped<TagService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<CommentService>();
        services.AddScoped<MailJobService>();
        services.AddScoped<MailingService>();
        services.AddScoped<ISmtpClient, SmtpClient>();

        services.Configure<MailingServiceOptions>(configuration.GetSection("MailingService"));
    }
}