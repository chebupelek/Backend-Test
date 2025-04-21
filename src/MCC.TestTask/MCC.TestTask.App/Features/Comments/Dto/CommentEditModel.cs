using System.ComponentModel.DataAnnotations;

namespace MCC.TestTask.App.Features.Comments.Dto;

public class CommentEditModel
{
    [Length(1, 1000)] [Required] public string Content { get; set; }
}