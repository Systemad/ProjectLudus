namespace Ludus.Server.Features.Common.Users.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public Guid? AvatarImageId { get; set; }

    public UserImage? UserImage { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public string AvatarUrl() =>
        AvatarImageId.HasValue ? $"api/images/{AvatarImageId}" : "/img/profiles/default_avatar.png";
}
