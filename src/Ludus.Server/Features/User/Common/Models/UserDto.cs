namespace Ludus.Server.Features.User.Common.Models;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public Guid? AvatarImageId { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public string AvatarUrl() =>
        AvatarImageId.HasValue ? $"api/images/{AvatarImageId}" : "/img/profiles/default_avatar.png";

    public UserImage? UserImage { get; set; }
}
