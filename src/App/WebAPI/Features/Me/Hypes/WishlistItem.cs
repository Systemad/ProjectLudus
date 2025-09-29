using Shared.Features.Games;
using Shared.Features.References.Platform;

namespace Me.Hypes;

public class HypedItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ArtworkImageId { get; set; }
    public string CoverImageId { get; set; }
    public long FirstReleaseDate { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<DateTime> ReleaseDates { get; set; }
    public GameType? GameType { get; set; }
    public bool IsHyped { get; set; }
}
