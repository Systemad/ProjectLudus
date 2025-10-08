using NodaTime;
using Shared.Features;
using SyncService.Data.Features;
using SyncService.Data.Features.Companies;
using SyncService.Data.Features.GameEngines;
using SyncService.Data.Features.Genres;
using SyncService.Data.Features.Platforms;
using SyncService.Data.Features.Themes;

namespace SyncService.Data.Entities;

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
