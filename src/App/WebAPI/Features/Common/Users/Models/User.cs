namespace WebAPI.Features.Common.Users.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public UserImage UserImage { get; set; } = null!;
    public DateTime CreatedDate { get; set; }

    public ICollection<GameHype> Hypes { get; } = new List<GameHype>();
    public ICollection<GameWishlist> Wishlists { get; } = new List<GameWishlist>();
    public ICollection<GameList> Lists { get; } = new List<GameList>();
}

public class UserImage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class GameWishlist
{
    public Guid Id { get; set; }
    public long GameId { get; set; }
    public DateTimeOffset WishlistedAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class GameHype
{
    public Guid Id { get; set; }
    public long GameId { get; set; }
    public DateTimeOffset HypeAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class GameFavorite
{
    public Guid Id { get; set; }
    public long GameId { get; set; }
    public DateTimeOffset FavoritedAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class GameList
{
    public Guid Id { get; set; }
    public bool Public { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<GameListItem> Games { get; set; } = new List<GameListItem>();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class GameListItem
{
    public Guid Id { get; set; }
    public long GameId { get; set; }
    public DateTimeOffset AddedAt { get; set; }

    public Guid GameListId { get; set; }
    public GameList GameList { get; set; } = null!;
}