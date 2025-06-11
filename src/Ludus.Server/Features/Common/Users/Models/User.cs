namespace Ludus.Server.Features.Common.Users.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public UserImage? UserImage { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public ICollection<GameHype> Hypes { get; } = new List<GameHype>();
    public ICollection<GameWishlist> Wishlists { get; } = new List<GameWishlist>();
    //public ICollection<GameFavorite> Favorites { get; } = new List<GameFavorite>();
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
    public long GameId { get; set; }
    public DateTimeOffset WishlistedAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class GameHype
{
    public long GameId { get; set; }
    public DateTimeOffset HypeAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class GameFavorite
{
    public long GameId { get; set; }
    public DateTimeOffset FavoritedAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
