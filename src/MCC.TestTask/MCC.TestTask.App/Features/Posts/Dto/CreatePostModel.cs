using System.ComponentModel.DataAnnotations;

namespace MCC.TestTask.App.Features.Posts.Dto;

public class CreatePostModel
{
    [Required] [Length(5, 1000)] public string Title { get; set; }

    [Required] [Length(5, 5000)] public string Description { get; set; }

    [Required] public int ReadingTime { get; set; }

    [MaxLength(1000)] public string? Image { get; set; }

    public Guid? AddressId { get; set; }

    [Required] [MinLength(1)] public List<Guid> Tags { get; set; }
}