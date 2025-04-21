using MCC.TestTask.Infrastructure;
using FluentResults;

namespace MCC.TestTask.App.Services.Auth;

public class SessionService
{
    private readonly List<Session> _sessions = [];

    public IList<Session> GetSessions(Guid userId)
    {
        _sessions.RemoveAll(s => s.ExpiresAfter < DateTime.UtcNow);
        return _sessions.Where(s => s.UserId == userId).ToList();
    }

    public Result<Session> GetSession(Guid sessionId)
    {
        _sessions.RemoveAll(s => s.ExpiresAfter < DateTime.UtcNow);
        var session = _sessions.FirstOrDefault(s => s.Id == sessionId);
        return session != null ? Result.Ok(session) : CustomErrors.NotFound("Session not found");
    }

    public Session CreateNewSession(Guid userId, TimeSpan lifetime)
    {
        var sessionId = Guid.NewGuid();
        while (_sessions.Any(s => s.Id == sessionId))
            sessionId = Guid.NewGuid();

        var session = new Session { Id = sessionId, UserId = userId, ExpiresAfter = DateTime.UtcNow.Add(lifetime) };
        _sessions.Add(session);
        return session;
    }

    public Result DeleteSession(Guid sessionId, Guid userId)
    {
        var session = _sessions.FirstOrDefault(s => s.Id == sessionId);
        if (session == null || session.UserId != userId)
            return CustomErrors.NotFound("Session not found");

        _sessions.Remove(session);
        return Result.Ok();
    }

    public void ClearSessions(Guid userId)
    {
        _sessions.RemoveAll(s => s.UserId == userId);
    }

    public Result UpdateRefreshToken(Guid sessionId, string refreshToken, DateTime expiresAt)
    {
        var session = _sessions.FirstOrDefault(s => s.Id == sessionId);
        if (session == null)
            return CustomErrors.NotFound("Session not found");

        session.RefreshToken = refreshToken;
        session.ExpiresAfter = expiresAt;
        return Result.Ok();
    }
}

public class Session
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string LastIp { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime ExpiresAfter { get; set; }
}