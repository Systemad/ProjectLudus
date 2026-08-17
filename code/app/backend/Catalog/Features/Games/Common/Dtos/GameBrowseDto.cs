namespace Catalog.Features.Games.Common.Dtos;

public sealed record GameBrowseDto
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public DateOnly? FirstReleaseDate { get; init; }

    public string? CoverUrl { get; init; }

    public SteamData? Steam { get; init; }

    public required GameFeatures GameFeatures { get; init; }

    public required IReadOnlyList<PlatformDto> Platforms { get; init; }

    public required IReadOnlyList<InvolvedCompanyDto> Companies { get; init; }
}

public sealed record SteamData
{
    public required string SteamAppId { get; init; }
    public long? CurrentPlayers { get; init; }
    public long? Peak24h { get; init; }
    public string? HeaderUrl { get; init; }
    public string? CapsuleUrl { get; init; }
    public SteamPricingData? Pricing { get; init; }
    public SteamReviewData? Review { get; init; }
}

public sealed record SteamPricingData
{
    public int? FinalCents { get; init; }
    public int? DiscountPercent { get; init; }
    public string? Currency { get; init; }
    public int? InitialCents { get; init; }
    public string? InitialFormatted { get; init; }
    public string? FinalFormatted { get; init; }
    public int? High30d { get; init; }
    public int? Low30d { get; init; }
}

public sealed record SteamReviewData
{
    public int? Score { get; init; }
    public string? Desc { get; init; }
    public int? TotalReviews { get; init; }
    public int? Positive { get; init; }
    public int? Negative { get; init; }
}

public sealed record GameFeatures(IReadOnlyList<Feature> Genres, IReadOnlyList<Feature> Themes);

public sealed record PlatformDto(string Id, string Name, string Slug);

public sealed record InvolvedCompanyDto(
    string Id,
    string CompanyId,
    string CompanyName,
    string CompanySlug,
    string CompanyLogoImageId,
    bool Developer,
    bool Publisher,
    bool Porting,
    bool Supporting
);

public sealed record Feature(string Name, string Slug);

public sealed record WebsiteDto(string Name, string? Type, string Url, bool? Trusted);
