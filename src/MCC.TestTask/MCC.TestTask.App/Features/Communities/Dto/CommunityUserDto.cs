namespace MCC.TestTask.App.Features.Communities.Dto;

public class CommunityUserDto
{
    public Guid UserId { get; set; }

    public Guid CommunityId { get; set; }

    public CommunityUserRole Role { get; set; }
}