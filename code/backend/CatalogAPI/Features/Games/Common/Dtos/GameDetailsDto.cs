namespace CatalogAPI.Features.Games.Common.Dtos;

public sealed record GameDetailsDto
{
    public long Id { get; init; }
    public string? Url { get; init; }
    public List<InvolvedCompanyDto>? InvolvedCompanies { get; init; }
    public List<string>? Themes { get; init; }
    public List<string>? GameModes { get; init; }
    public List<string>? PlayerPerspectives { get; init; }
    public List<WebsiteDto>? Websites { get; init; }
    public List<AlternativeNameDto>? AlternativeNames { get; init; }
    public List<GameEnginesDto>? GameEngines { get; init; }
    public List<LanguageSupportsDto>? LanguageSupports { get; set; }
    public List<FranchiseDto>? Franchises { get; set; }
}

public record InvolvedCompanyDto(string Name, string Slug, bool Published, bool Developed);

public record AlternativeNameDto(long Id, string? Name, string? Comment);

public record GameEnginesDto(long Id, string? Name, string? ImageId, string? Url);

public record LanguageSupportsDto(string Language, string? NativeName, string? Type);

public record FranchiseDto(string Name, string Slug);

public record WebsiteDto(string? Name, string? Type, string? Url, bool? Trusted);
