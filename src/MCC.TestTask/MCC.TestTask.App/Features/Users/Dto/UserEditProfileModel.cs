using System.ComponentModel.DataAnnotations;
using MCC.TestTask.Domain;

namespace MCC.TestTask.App.Features.Users.Dto;

public class UserEditProfileModel
{
    [Required] [Length(1, 1000)] public string FullName { get; set; }

    public DateOnly? BirthDate { get; set; }

    [Required] public Gender Gender { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Phone] public string? PhoneNumber { get; set; }
}