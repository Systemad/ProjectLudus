using Shared.Features.Games;
using Shared.Features.IGDB;
using Shared.Features.References.Platform;

namespace Me.Wishlists;

public class WishlistItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ArtworkImageId { get; set; }
    public string CoverImageId { get; set; }
    public long FirstReleaseDate { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<DateTime> ReleaseDates { get; set; }
    public GameType? GameType { get; set; }
    public bool IsWishlisted { get; set; }
}
