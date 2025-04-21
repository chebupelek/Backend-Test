using System.ComponentModel.DataAnnotations;
using MCC.TestTask.Domain;

namespace MCC.TestTask.App.Features.Users.Dto;

public class UserRegisterModel : IValidatableObject
{
    [Required] [Length(1, 1000)] public string FullName { get; set; }

    public DateOnly? BirthDate { get; set; }

    [Required] public Gender Gender { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Phone] public string? PhoneNumber { get; set; }

    [Required] [MinLength(6)] public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BirthDate != null && BirthDate < DateOnly.FromDateTime(new DateTime(1900, 1, 1)))
            yield return new ValidationResult("Birth Date is invalid", [nameof(BirthDate)]);
    }
}