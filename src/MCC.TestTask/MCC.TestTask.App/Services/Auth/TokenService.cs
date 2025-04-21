using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MCC.TestTask.Domain;
using MCC.TestTask.Infrastructure;
using FluentResults;
using Microsoft.IdentityModel.Tokens;

namespace MCC.TestTask.App.Services.Auth;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Result<string> IssueAccessToken(User user, Session session)
    {
        return GenerateToken(user, session, DateTime.UtcNow.AddHours(12));
    }

    public Result<string> IssueRefreshToken(User user, Session session, string? previousRefreshToken = null)
    {
        return GenerateToken(user, session, DateTime.UtcNow.AddDays(7));
    }

    public Result CheckAccessToken(string accessToken)
    {
        return ValidateToken(accessToken).ToResult();
    }

    public Result CheckRefreshToken(string refreshToken)
    {
        return ValidateToken(refreshToken).ToResult();
    }

    private Result<ClaimsPrincipal> ValidateToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return Result.Try(() => tokenHandler.ValidateToken(token, tokenValidationParameters, out _),
            e => new ExceptionalError(e.Message, e));
    }

    private Result<string> GenerateToken(User user, Session session, DateTime expirationDate)
    {
        var claims = new[]
        {
            new Claim(BlogClaimTypes.UserId, user.Id.ToString()),
            new Claim(BlogClaimTypes.SessionId, session.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Subject = new ClaimsIdentity(claims),
            Expires = expirationDate,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
        };

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);
        return Result.Try(() => handler.WriteToken(token), e => new ExceptionalError(e.Message, e));
    }

    // public string GenerateAccessToken(IEnumerable<Claim> claims)
    // {
    //     var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //     var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    //
    //     var tokeOptions = new JwtSecurityToken(
    //         _configuration["Jwt:Issuer"],
    //         _configuration["Jwt:Audience"],
    //         claims,
    //         expires: DateTime.Now.AddMinutes(5),
    //         signingCredentials: signinCredentials
    //     );
    //
    //     return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    // }
    //
    // public string GenerateRefreshToken()
    // {
    //     var randomNumber = new byte[32];
    //     using (var rng = RandomNumberGenerator.Create())
    //     {
    //         rng.GetBytes(randomNumber);
    //         return Convert.ToBase64String(randomNumber);
    //     }
    // }
    //

    public Result<Guid> GetUserIdFromToken(string token)
    {
        return GetPrincipalFromExpiredToken(token)
            .Map(principal =>
                principal?.Claims.FirstOrDefault(x => x.Type == BlogClaimTypes.UserId))
            .Bind(sessionIdClaim => sessionIdClaim != null
                ? Guid.Parse(sessionIdClaim.Value)
                : Result.Fail<Guid>(new ValidationError("Invalid token")));
    }

    public Result<Guid> GetSessionIdFromToken(string token)
    {
        return GetPrincipalFromExpiredToken(token)
            .Map(principal =>
                principal?.Claims.FirstOrDefault(x => x.Type == BlogClaimTypes.SessionId))
            .Bind(sessionIdClaim => sessionIdClaim != null
                ? Guid.Parse(sessionIdClaim.Value)
                : Result.Fail<Guid>(new ValidationError("Invalid token")));
    }

    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            return Result.Fail(new ValidationError("Invalid token"));

        return principal;
    }
}