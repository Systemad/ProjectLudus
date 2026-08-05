using System.ComponentModel.DataAnnotations;

namespace Catalog.Features.Games.Common.Dtos;

public class GameBrowseDto
{
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    //[Required] public string Status { get; set; } = string.Empty;

    public DateOnly? FirstReleaseDate { get; set; }

    public string? CoverUrl { get; set; }

    public SteamData? Steam { get; set; }

    public SteamPricingData? Pricing { get; set; }

    public SteamReviewData? Review { get; set; }

    [Required]
    public GameFeatures GameFeatures { get; set; } = new([], []);

    [Required]
    public List<PlatformDto> Platforms { get; set; } = [];

    [Required]
    public List<InvolvedCompanyDto> Companies { get; set; } = [];
}

public sealed record SteamData(
    long? SteamAppId,
    long? CurrentPlayers,
    long? Peak24h,
    string? HeaderUrl,
    string? CapsuleUrl
);

public sealed record SteamPricingData(
    int? FinalCents,
    int? DiscountPercent,
    string? Currency,
    int? InitialCents,
    string? InitialFormatted,
    string? FinalFormatted,
    int? High30d,
    int? Low30d
);

public sealed record SteamReviewData(
    int? Score,
    string? Desc,
    int? TotalReviews,
    int? Positive,
    int? Negative
);

public sealed record GameFeatures(List<Feature> Genres, List<Feature> Themes);

public sealed record PlatformDto(long Id, string Name, string Slug);

public sealed record InvolvedCompanyDto(
    long Id,
    long CompanyId,
    string CompanyName,
    string CompanySlug,
    string CompanyLogoImageId,
    bool Developer,
    bool Publisher,
    bool Porting,
    bool Supporting
);

public record Feature([Required] string Name, [Required] string Slug);

public record WebsiteDto(string Name, string? Type, string Url, bool? Trusted);
