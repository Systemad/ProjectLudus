using Catalog.Companies.Models;
using Catalog.Franchises.Models;
using Catalog.GameEngines.Models;
using Catalog.GameModes.Models;
using Catalog.Genres.Models;
using Catalog.Platforms.Models;
using Catalog.PlayerPerspective.Models;
using Catalog.Themes.Models;
using IGDB.Models;
using NodaTime;

namespace Catalog.Games.Models;

public class GameEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public long GameType { get; set; }
    public double? Rating { get; set; }
    public long? RatingCount { get; set; }
    public double? TotalRating { get; set; }
    public long? TotalRatingCount { get; set; }
    public string Url { get; set; }
    public Instant FirstReleaseDate { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant CreatedAt { get; set; }
    public Game Metadata { get; set; } = null!;
    public ICollection<GameEntity> SimilarGames { get; set; } = [];
    public ICollection<CompanyEntity> Developers { get; set; } = [];
    public ICollection<CompanyEntity> Publishers { get; set; } = [];
    public ICollection<GameEngineEntity> GameEngines { get; set; } = [];
    public ICollection<GenreEntity> Genres { get; set; } = [];
    public ICollection<PlatformEntity> Platforms { get; set; } = [];
    public ICollection<FranchiseEntity> Franchises { get; set; } = [];
    public ICollection<ThemeEntity> Themes { get; set; } = [];
    public ICollection<GameModeEntity> GameModes { get; set; } = [];
    public ICollection<PlayerPerspectiveEntity> PlayerPerspectives { get; set; } = [];
}
