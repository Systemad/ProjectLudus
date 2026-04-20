using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Features.Games.GetOverview;

public class GameOverviewDto
{
    public long Id { get; set; }
    public string? Slug { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Summary { get; set; }
    public string? Storyline { get; set; }
    public string? Cover { get; set; }
    public string? CoverUrl { get; set; }
    public long? GameType { get; set; }
    public string? GameTypeName { get; set; }

    [Required]
    public required List<string> Genres { get; set; } = [];

    [Required]
    public required List<string> Themes { get; set; } = [];

    public bool IsReleased { get; set; }

    [Required]
    public required List<PlatformsDto> Platforms { get; set; } = [];

    [Required]
    public required List<ReleaseDatePlatformDto> ReleaseDatePlatform { get; set; } = [];

    [Required]
    public required List<ReleaseDatesDto> ReleaseDates { get; set; } = [];
}

public class Rating
{
    public long Critics { get; set; }
}

public record ReleaseDatePlatformDto(long? ReleaseDate, string? Platform);

public record PlatformsDto([Required] string Name, [Required] string? Slug);

public record ReleaseDatesDto(long? ReleaseDate, string? Region);
