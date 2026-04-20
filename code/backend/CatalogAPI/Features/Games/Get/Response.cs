using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Features.Games.Get;

public sealed record GameDetailsDto
{
    [Required]
    public required long Id { get; set; }

    [Required]
    public required string Url { get; set; }

    [Required]
    public required List<InvolvedCompanyDto> InvolvedCompanies { get; set; } = [];

    [Required]
    public required List<string> Themes { get; set; } = [];

    [Required]
    public required List<string> GameModes { get; set; } = [];

    [Required]
    public required List<string> PlayerPerspectives { get; set; } = [];

    [Required]
    public required List<WebsiteDto> Websites { get; set; } = [];

    [Required]
    public required List<AlternativeNameDto> AlternativeNames { get; set; } = [];

    [Required]
    public required List<GameEnginesDto> GameEngines { get; set; } = [];

    [Required]
    public required List<LanguageSupportsDto> LanguageSupports { get; set; } = [];

    [Required]
    public required List<FranchiseDto> Franchises { get; set; } = [];
}

public record InvolvedCompanyDto(string Name, string Slug, bool Published, bool Developed);

public record AlternativeNameDto(long Id, string Name, string? Comment);

public record GameEnginesDto(long Id, string Name, string? ImageId, string? Url);

public record LanguageSupportsDto(string Language, string? NativeName, string? Type);

public record FranchiseDto(string Name, string Slug);

public record WebsiteDto(string Name, string? Type, string Url, bool? Trusted);
