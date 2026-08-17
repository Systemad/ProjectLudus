using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Browse.Get;

/// <summary>Response containing detailed game information.</summary>
public sealed record GetGameResponse(GameDetailsDto Game);

public sealed record GameDetailsDto
{
    public required string Id { get; init; }

    public required string Url { get; init; }

    public required IReadOnlyList<InvolvedCompanyDto> InvolvedCompanies { get; init; }

    public required IReadOnlyList<string> Themes { get; init; }

    public required IReadOnlyList<string> GameModes { get; init; }

    public required IReadOnlyList<string> PlayerPerspectives { get; init; }

    public required IReadOnlyList<WebsiteDto> Websites { get; init; }

    public required IReadOnlyList<AlternativeNameDto> AlternativeNames { get; init; }

    public required IReadOnlyList<GameEnginesDto> GameEngines { get; init; }

    public required IReadOnlyList<LanguageSupportsDto> LanguageSupports { get; init; }

    public required IReadOnlyList<FranchiseDto> Franchises { get; init; }
}

public sealed record AlternativeNameDto(string Id, string Name, string? Comment);

public sealed record GameEnginesDto(string Id, string Name, string? ImageId, string? Url);

public sealed record LanguageSupportsDto(string Language, string? NativeName, string? Type);

public sealed record FranchiseDto(string Name, string Slug);
