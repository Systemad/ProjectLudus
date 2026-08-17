using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Browse.GetOverview;

public sealed record GameOverviewDto
{
    public required string Id { get; init; }
    public string? Slug { get; init; }

    public required string Name { get; init; }

    public string? Summary { get; init; }
    public string? Storyline { get; init; }
    public string? Cover { get; init; }
    public string? CoverUrl { get; init; }
    public string? GameType { get; init; }
    public string? GameTypeName { get; init; }
    public SteamData? Steam { get; init; }

    public required IReadOnlyList<string> Genres { get; init; }

    public required IReadOnlyList<string> Themes { get; init; }

    public bool IsReleased { get; init; }

    public required IReadOnlyList<PlatformsDto> Platforms { get; init; }

    public required IReadOnlyList<ReleaseDatePlatformDto> ReleaseDatePlatform { get; init; }

    public required IReadOnlyList<ReleaseDatesDto> ReleaseDates { get; init; }
}

public sealed record ReleaseDatePlatformDto(long? ReleaseDate, string? Platform);

public sealed record PlatformsDto(string Name, string? Slug);

public sealed record ReleaseDatesDto(long? ReleaseDate, string? Region);

public sealed record GetGameOverviewResponse(GameOverviewDto Game);
