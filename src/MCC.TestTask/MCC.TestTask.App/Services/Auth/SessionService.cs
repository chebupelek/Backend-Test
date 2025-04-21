using MCC.TestTask.Infrastructure;
using FluentResults;
using System;
using MCC.TestTask.Persistance;
using Microsoft.EntityFrameworkCore;
using MCC.TestTask.Domain;

namespace MCC.TestTask.App.Services.Auth;

public class SessionService
{
    private readonly BlogDbContext _blogDbContext;

    public SessionService(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public IList<Session> GetSessions(Guid userId)
    {
        var now = DateTime.UtcNow;
        var expiredSessions = _blogDbContext.Sessions.Where(s => s.ExpiresAfter < now);
        _blogDbContext.Sessions.RemoveRange(expiredSessions);
        _blogDbContext.SaveChanges();

        return _blogDbContext.Sessions
            .Where(s => s.UserId == userId && s.ExpiresAfter > now)
            .ToList();
    }

    public Result<Session> GetSession(Guid sessionId)
    {
        var now = DateTime.UtcNow;
        var expiredSessions = _blogDbContext.Sessions.Where(s => s.ExpiresAfter < now);
        _blogDbContext.Sessions.RemoveRange(expiredSessions);
        _blogDbContext.SaveChanges();

        var session = _blogDbContext.Sessions.FirstOrDefault(s => s.Id == sessionId && s.ExpiresAfter > DateTime.UtcNow);
        return session != null ? Result.Ok(session) : CustomErrors.NotFound("Session not found");
    }

    public Session CreateNewSession(Guid userId, TimeSpan lifetime)
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ExpiresAfter = DateTime.UtcNow.Add(lifetime)
        };

        _blogDbContext.Sessions.Add(session);
        _blogDbContext.SaveChanges();

        return session;
    }

    public Result DeleteSession(Guid sessionId, Guid userId)
    {
        var session = _blogDbContext.Sessions.FirstOrDefault(s => s.Id == sessionId && s.UserId == userId);
        if (session == null)
            return CustomErrors.NotFound("Session not found");

        _blogDbContext.Sessions.Remove(session);
        _blogDbContext.SaveChanges();

        return Result.Ok();
    }

    public void ClearSessions(Guid userId)
    {
        var userSessions = _blogDbContext.Sessions.Where(s => s.UserId == userId);
        _blogDbContext.Sessions.RemoveRange(userSessions);
        _blogDbContext.SaveChanges();
    }

    public Result UpdateRefreshToken(Guid sessionId, string refreshToken, DateTime expiresAt)
    {
        var session = _blogDbContext.Sessions.FirstOrDefault(s => s.Id == sessionId);
        if (session == null)
            return CustomErrors.NotFound("Session not found");

        session.RefreshToken = refreshToken;
        session.ExpiresAfter = expiresAt;

        _blogDbContext.SaveChanges();
        return Result.Ok();
    }
}