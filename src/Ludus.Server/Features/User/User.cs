using Ludus.Server.Features.User.List;
using Ludus.Server.Features.User.Status;

namespace Ludus.Server.Features.User;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public int? AvatarImageId { get; set; }

    public DateTime CreateDate { get; set; }

    public string AvatarUrl() =>
        AvatarImageId.HasValue ? $"api/images/{AvatarImageId}" : "/img/profiles/default_avatar.png";

    public UserImage? UserImage { get; set; }
    public List<UserGameStatus> GameStatuses { get; set; }
    public List<UserGameList> GameLists { get; set; }
}
