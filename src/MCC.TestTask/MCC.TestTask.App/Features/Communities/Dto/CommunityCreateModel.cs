using System.ComponentModel.DataAnnotations;
using MCC.TestTask.Domain;

namespace MCC.TestTask.App.Features.Communities.Dto;

public class CommunityCreateModel
{
    [Length(1, 100)] public string Name { get; init; }

    [Length(1, 1000)] public string Description { get; init; }

    [Required] public CommunityType CommunityType { get; init; }
}