using Shared.Features.Games;
using Shared.Features.IGDB;
using Shared.Features.References.Platform;

namespace Ludus.Api.Features.Common.Games.Models;

public class GameDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ArtworkImageId { get; set; }
    public string CoverImageId { get; set; }
    public long FirstReleaseDate { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<DateTime> ReleaseDates { get; set; }
    public IgdbGameType? GameType { get; set; }
}
