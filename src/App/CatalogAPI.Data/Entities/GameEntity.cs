using CatalogAPI.Data.Features.Companies;
using CatalogAPI.Data.Features.GameEngines;
using CatalogAPI.Data.Features.Genres;
using CatalogAPI.Data.Features.Platforms;
using CatalogAPI.Data.Features.Themes;
using NodaTime;
using Shared.Features;
using CatalogAPI.Data.Features;

namespace CatalogAPI.Data.Entities;

public partial class GameEntity
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

    public IgdbGame Metadata { get; set; } = null!;
    public virtual ICollection<GameEngineEntity> GameEngines { get; set; } = new List<GameEngineEntity>();
    public virtual ICollection<GenreEntity> Genres { get; set; } = new List<GenreEntity>();
    public virtual ICollection<PlatformEntity> Platforms { get; set; } = new List<PlatformEntity>();
    public virtual ICollection<ThemeEntity> Themes { get; set; } = new List<ThemeEntity>();
    public virtual List<CompanyEntity> Companies { get; set; } = [];
}
