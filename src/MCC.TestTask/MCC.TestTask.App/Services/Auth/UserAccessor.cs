using System.Security.Claims;
using System.Security.Principal;
using MCC.TestTask.Infrastructure;
using FluentResults;

namespace MCC.TestTask.App.Services.Auth;

public class UserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(
        IHttpContextAccessor httpContextAccessor
    )
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsHttpContextAvailable => _httpContextAccessor.HttpContext != null;

    public bool IsUserAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
        && !IsAuthenticationInProgress;

    private bool IsAuthenticationInProgress =>
        IsHttpContextAvailable
        && _httpContextAccessor.HttpContext?.Request.Path.StartsWithSegments("/connect") == true;

    public Result<Guid> GetUserId()
    {
        return GetUserIdentity()
            .Bind(user => GetClaimValue(user, BlogClaimTypes.UserId))
            .Bind(value => Result.Ok(Guid.Parse(value)));
    }

    public Result<Guid> GetSessionId()
    {
        return GetUserIdentity()
            .Bind(user => GetClaimValue(user, BlogClaimTypes.SessionId))
            .Bind(value => Result.Ok(Guid.Parse(value)));
    }

    private Result<string> GetClaimValue(IIdentity identity, string claimType)
    {
        if (identity is not ClaimsIdentity id)
            return CustomErrors.AuthError($"Identity {identity} is not ClaimsIdentity");

        var claim = id.FindFirst(claimType);

        return claim?.Value != null ? claim.Value : CustomErrors.AuthError($"Claim '{claimType}' is missing");
    }

    private Result<IIdentity> GetUserIdentity()
    {
        var identity = _httpContextAccessor.HttpContext?.User?.Identity;
        return identity == null ? CustomErrors.AuthError("Unable to get user identity from HttpContext") : Result.Ok(identity);
    }
}