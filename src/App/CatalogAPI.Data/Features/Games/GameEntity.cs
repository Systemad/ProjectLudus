using CatalogAPI.Data.Features.Companies;
using CatalogAPI.Data.Features.GameEngines;
using CatalogAPI.Data.Features.GameModes;
using CatalogAPI.Data.Features.Genres;
using CatalogAPI.Data.Features.Platforms;
using CatalogAPI.Data.Features.Themes;
using IGDB.Models;
using NodaTime;

namespace CatalogAPI.Data.Features.Games;

public class GameEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long GameType { get; set; }
    public double? Rating { get; set; }
    public long? RatingCount { get; set; }
    public double? TotalRating { get; set; }
    public long? TotalRatingCount { get; set; }
    public Instant FirstReleaseDate { get; set; }
    public Instant UpdatedAt { get; set; }

    public Game Metadata { get; set; } = null!;
    public ICollection<GameEntity> SimilarGames { get; set; } = [];
    public ICollection<CompanyEntity> Companies { get; set; } = [];
    public ICollection<GameEngineEntity> GameEngines { get; set; } = [];
    public ICollection<GenreEntity> Genres { get; set; } = [];
    public ICollection<PlatformEntity> Platforms { get; set; } = [];
    public ICollection<ThemeEntity> Themes { get; set; } = [];
    public ICollection<GameModeEntity> GameModes { get; set; } = [];
}
