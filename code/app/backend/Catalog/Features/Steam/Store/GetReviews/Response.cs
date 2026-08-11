namespace Catalog.Features.Steam.Store.GetReviews;

public sealed record GetReviewsResponse(
    string GameId,
    string? SteamAppId,
    int? NumReviews,
    int? ReviewScore,
    string? ReviewScoreDesc,
    int? TotalPositive,
    int? TotalNegative,
    int? TotalReviews
);
