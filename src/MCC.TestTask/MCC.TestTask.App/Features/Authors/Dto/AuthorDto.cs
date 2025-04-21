using MCC.TestTask.Domain;

namespace MCC.TestTask.App.Features.Authors.Dto;

public class AuthorDto
{
    public string FullName { get; set; }

    public DateOnly? Birthdate { get; set; }

    public Gender Gender { get; set; }

    public int Posts { get; set; }

    public int Likes { get; set; }

    public DateTime Created { get; set; }
}