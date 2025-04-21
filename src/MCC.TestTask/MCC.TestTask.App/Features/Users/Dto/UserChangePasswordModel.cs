using System.ComponentModel.DataAnnotations;

namespace MCC.TestTask.App.Features.Users.Dto;

public class UserChangePasswordModel : IValidatableObject
{
    [Required] public string Email { get; set; }

    [Required] public string OldPassword { get; set; }

    [Required] [MinLength(6)] public string NewPassword { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NewPassword == OldPassword)
            yield return new ValidationResult("Passwords must not match");
    }
}