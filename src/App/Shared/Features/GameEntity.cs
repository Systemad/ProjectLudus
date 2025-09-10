using NodaTime;

namespace Shared.Features;

public class GameEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Instant ReleaseDate { get; set; } 
    public long GameType { get; set; }
    public long[] Platforms { get; set; }
    public long[] GameEngines { get; set; }
    public long[] Genres { get; set; }
    public long[] Themes { get; set; }
    public double Rating { get; set; }
    public long RatingCount { get; set; }
    public double TotalRating { get; set; }
    public long TotalRatingCount { get; set; }
    public long UpdatedAt { get; set; }
    public IGDBGame RawData { get; set; }
}