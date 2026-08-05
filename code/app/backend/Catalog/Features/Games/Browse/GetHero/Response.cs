using System.ComponentModel.DataAnnotations;
using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Browse.GetHero;

public class GameHeroDto
{
    public long Id { get; set; }

    public string? Slug { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Summary { get; set; }

    public string? Cover { get; set; }

    public string? CoverUrl { get; set; }

    public string? GameTypeName { get; set; }

    public DateOnly? FirstReleaseDate { get; set; }

    [Required]
    public required List<Feature> Genres { get; set; } = [];

    [Required]
    public required List<Feature> Themes { get; set; } = [];

    [Required]
    public required List<Feature> GameModes { get; set; } = [];

    [Required]
    public required List<Feature> Keywords { get; set; } = [];

    [Required]
    public required List<Feature> PlayerPerspectives { get; set; } = [];

    [Required]
    public required List<PlatformDto> Platforms { get; set; } = [];

    [Required]
    public required List<InvolvedCompanyDto> Companies { get; set; } = [];
}

public sealed record GetGameHeroResponse(GameHeroDto Game);
