using System.ComponentModel.DataAnnotations;

namespace MCC.TestTask.App.Features.Tags.Dto;

public class CreateTagModel
{
    [Required] [Length(3, 20)] public string Name { get; set; }
}