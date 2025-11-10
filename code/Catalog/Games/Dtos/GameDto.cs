using IGDB.Models;
using NodaTime;

namespace Catalog.Games.Dtos;

public class GameDto
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
    //public virtual ICollection<GameEngineEntity> GameEngines { get; set; } = new List<GameEngineEntity>();
    //public virtual ICollection<GenreEntity> Genres { get; set; } = new List<GenreEntity>();
    //public virtual ICollection<PlatformEntity> Platforms { get; set; } = new List<PlatformEntity>();
    //public virtual ICollection<ThemeEntity> Themes { get; set; } = new List<ThemeEntity>();
    //public virtual ICollection<GameModeEntity> GameModes { get; set; } = new List<GameModeEntity>();
    //public virtual ICollection<CompanyEntity> Companies { get; set; } = new List<CompanyEntity>();
}