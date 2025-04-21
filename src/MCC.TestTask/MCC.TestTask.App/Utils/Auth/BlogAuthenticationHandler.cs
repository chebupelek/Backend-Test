using System.Security.Claims;
using System.Text.Encodings.Web;
using MCC.TestTask.App.Features.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MCC.TestTask.App.Utils.Auth;

public class BlogAuthenticationHandler : AuthenticationHandler<BlogAuthenticationOptions>
{
    private readonly UserService _userService;

    public BlogAuthenticationHandler(
        IOptionsMonitor<BlogAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        UserService userService)
        : base(options, logger, encoder)
    {
        _userService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        var validationRequired = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

        if (!Request.Headers.TryGetValue(Options.TokenHeaderName, out var tokens))
            return validationRequired
                ? AuthenticateResult.Fail("Missing Authorization Header")
                : AuthenticateResult.NoResult();

        if (tokens.Count != 1)
            return validationRequired
                ? AuthenticateResult.Fail("Invalid Authorization Header")
                : AuthenticateResult.NoResult();

        var token = tokens.First()?[7..];

        if (string.IsNullOrWhiteSpace(token))
            return validationRequired
                ? AuthenticateResult.Fail("Invalid Token")
                : AuthenticateResult.NoResult();

        var validateAndParseResult = await _userService.ValidateAndParseAccessToken(token);

        if (!validateAndParseResult.IsSuccess)
            return AuthenticateResult.Fail(validateAndParseResult.Errors.First().Message);

        var claims = validateAndParseResult.Value;
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}