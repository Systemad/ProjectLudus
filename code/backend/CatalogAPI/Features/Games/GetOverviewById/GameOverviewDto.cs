namespace CatalogAPI.Features.Games.GetOverviewById;

public class GameOverviewDto
{
    public long Id { get; init; }
    public string? Slug { get; init; }
    public string? Name { get; init; }
    public string? Summary { get; init; }
    public string? Storyline { get; init; }
    public string? Cover { get; init; }
    public string? CoverUrl { get; init; }
    public long? GameType { get; init; }
    public string? GameTypeName { get; init; }
    public List<string>? Genres { get; init; }
    public List<string>? Themes { get; init; }
    public bool IsReleased { get; set; }
    public List<PlatformsDto>? Platforms { get; init; }
    public List<ReleaseDatePlatformDto>? ReleaseDatePlatform { get; set; }
    public List<ReleaseDatesDto>? ReleaseDates { get; set; }
}

// Ignore Rating for now, Do not remove
public class Rating
{
    public long Critics { get; set; }
}

public record ReleaseDatePlatformDto(long? ReleaseDate, string? Platform);
public record PlatformsDto(string? Name, string? Slug);

public record ReleaseDatesDto(long? ReleaseDate, string? Region);
