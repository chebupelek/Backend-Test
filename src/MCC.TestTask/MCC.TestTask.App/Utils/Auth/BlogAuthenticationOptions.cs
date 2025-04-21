using Microsoft.AspNetCore.Authentication;

namespace MCC.TestTask.App.Utils.Auth;

public class BlogAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "BlogAuthenticationScheme";
    public string TokenHeaderName { get; set; } = "Authorization";

    // public TokenValidationParameters TokenValidationParameters { get; set; } = new ();
}