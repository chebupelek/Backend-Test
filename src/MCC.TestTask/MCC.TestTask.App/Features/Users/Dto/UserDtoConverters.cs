using MCC.TestTask.Domain;

namespace MCC.TestTask.App.Features.Users.Dto;

public static class UserDtoConverters
{
    public static User ToUser(this UserRegisterModel model)
    {
        return new User
        {
            FullName = model.FullName,
            BirthDate = model.BirthDate,
            CreateTime = DateTime.UtcNow,
            Email = model.Email,
            Gender = model.Gender,
            PhoneNumber = model.PhoneNumber
        };
    }

    public static UserDto ToDto(this User model)
    {
        return new UserDto
        {
            Id = model.Id,
            FullName = model.FullName,
            BirthDate = model.BirthDate,
            CreateTime = model.CreateTime,
            Email = model.Email,
            Gender = model.Gender,
            PhoneNumber = model.PhoneNumber
        };
    }
}