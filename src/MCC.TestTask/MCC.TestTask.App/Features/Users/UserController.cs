using MCC.TestTask.App.Features.Users.Dto;
using MCC.TestTask.App.Services.Auth;
using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Features.Users;

[ApiController]
[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly UserAccessor _userAccessor;
    private readonly UserService _userService;

    public UserController(UserService userService, UserAccessor userAccessor)
    {
        _userService = userService;
        _userAccessor = userAccessor;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<LoginResultDto>> Register([FromBody] UserRegisterModel model)
    {
        return await _userService.RegisterUser(model).ToActionResult();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LoginResultDto> Login(UserLoginModel userLoginDto)
    {
        return _userService.LoginUser(userLoginDto).ToActionResult();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResultDto>> Refresh([FromBody] string refreshToken)
    {
        return await _userService.RefreshUserToken(refreshToken).ToActionResult();
    }

    [AllowAnonymous]
    [HttpPost("change-password")]
    public async Task<ActionResult> GetAllUsers([FromBody] UserChangePasswordModel model)
    {
        return await _userService.ChangeUserPassword(model).ToActionResult();
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        return await _userAccessor.GetUserId()
            .Bind(async Task<Result<UserDto>> (userId) => await _userService.GetUserById(userId))
            .ToActionResult();
    }


    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> EditProfile([FromBody] UserEditProfileModel model)
    {
        return await _userAccessor.GetUserId()
            .Bind(async Task<Result<UserDto>> (userId) => await _userService.EditUserById(userId, model))
            .ToActionResult();
    }
}