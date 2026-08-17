using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Browse.GetHero;

public sealed record GameHeroDto
{
    public required string Id { get; init; }

    public string? Slug { get; init; }

    public required string Name { get; init; }

    public string? Summary { get; init; }

    public string? Cover { get; init; }

    public string? CoverUrl { get; init; }

    public string? GameTypeName { get; init; }

    public DateOnly? FirstReleaseDate { get; init; }

    public required IReadOnlyList<Feature> Genres { get; init; }

    public required IReadOnlyList<Feature> Themes { get; init; }

    public required IReadOnlyList<Feature> GameModes { get; init; }

    public required IReadOnlyList<Feature> Keywords { get; init; }

    public required IReadOnlyList<Feature> PlayerPerspectives { get; init; }

    public required IReadOnlyList<PlatformDto> Platforms { get; init; }

    public required IReadOnlyList<InvolvedCompanyDto> Companies { get; init; }
}

public sealed record GetGameHeroResponse(GameHeroDto Game);
