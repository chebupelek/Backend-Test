using MCC.TestTask.App.Features.Sessions.Dto;
using MCC.TestTask.App.Services.Auth;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Features.Sessions;

[Route("api/session")]
[Authorize]
public class SessionsController : ControllerBase
{
    private readonly SessionService _sessionService;
    private readonly UserAccessor _userAccessor;

    public SessionsController(SessionService sessionService, UserAccessor userAccessor)
    {
        _sessionService = sessionService;
        _userAccessor = userAccessor;
    }

    [HttpGet]
    public ActionResult<List<SessionDto>> GetSessions()
    {
        return _userAccessor.GetUserId()
            .Map(userId => _sessionService.GetSessions(userId).Select(s => s.ToDto()).ToList())
            .ToActionResult();
    }

    [HttpGet("current")]
    public ActionResult<List<SessionDto>> GetCurrentSession()
    {
        return _userAccessor.GetSessionId()
            .Bind(sessionId => _sessionService.GetSession(sessionId).Map(s => s.ToDto()))
            .ToActionResult();
    }

    [HttpDelete("{id}")]
    public ActionResult EndSession(Guid id)
    {
        return _userAccessor.GetUserId()
            .Bind(userId => _sessionService.DeleteSession(id, userId))
            .ToActionResult();
    }
}