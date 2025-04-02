using Ludus.Shared.Features.Games;

namespace Ludus.Shared.Features.User;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public int? AvatarImageId { get; set; }

    public DateTime CreateDate { get; set; }

    public string AvatarUrl() =>
        AvatarImageId.HasValue ? $"api/images/{AvatarImageId}" : "/img/profiles/default_avatar.png";

    public UserImage? UserImage { get; set; }

    public List<UserGameStatus> GameStatusList { get; set; }
}

public class UserGameStatus
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public long GameId { get; set; }
    public GameStatus Status { get; set; }
}

public class UserImage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public DateTime CreateDate { get; set; }
    public int UserId { get; set; }
}
