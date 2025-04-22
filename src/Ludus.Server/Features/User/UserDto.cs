namespace Ludus.Server.Features.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public int? AvatarImageId { get; set; }

    public DateTime CreateDate { get; set; }

    public string AvatarUrl() =>
        AvatarImageId.HasValue ? $"api/images/{AvatarImageId}" : "/img/profiles/default_avatar.png";

    public UserImage? UserImage { get; set; }
}
