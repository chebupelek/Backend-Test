using System.ComponentModel.DataAnnotations;

namespace MCC.TestTask.App.Features.Users.Dto;

public class UserLoginModel
{
    [Required] [MinLength(1)] public string Email { get; set; }

    [Required] [MinLength(1)] public string Password { get; set; }
}