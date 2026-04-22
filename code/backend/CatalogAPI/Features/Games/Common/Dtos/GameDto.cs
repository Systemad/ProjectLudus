using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Features.Games.Common.Dtos;

public class GameDto
{
    [Required]
    public required long Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public DateOnly? FirstReleaseDate { get; set; }

    public string? GameType { get; set; }

    public string? GameStatus { get; set; }

    public string? CoverUrl { get; set; }

    [Required]
    public required List<Feature> Themes { get; set; } = [];

    [Required]
    public required List<Feature> Genres { get; set; } = [];

    [Required]
    public required List<Feature> GameModes { get; set; } = [];

    [Required]
    public required List<Feature> Platforms { get; set; } = [];

    [Required]
    public required List<Feature> Developers { get; set; } = [];
}

public record Feature([Required] string Name, [Required] string Slug);
