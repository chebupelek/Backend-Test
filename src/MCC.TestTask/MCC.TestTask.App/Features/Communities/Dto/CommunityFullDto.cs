using MCC.TestTask.App.Features.Comments.Dto;
using MCC.TestTask.App.Features.Users.Dto;

namespace MCC.TestTask.App.Features.Communities.Dto;

public class CommunityFullDto
{
    public Guid Id { get; set; }

    public DateTime CreateTime { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsClosed { get; set; }

    public int SubscribersCount { get; set; }

    public List<UserDto> Administrators { get; set; }
    
    public UserDto Creator { get; set; }
}