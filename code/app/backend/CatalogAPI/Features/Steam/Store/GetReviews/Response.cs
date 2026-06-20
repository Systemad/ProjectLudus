namespace CatalogAPI.Features.Steam.Store.GetReviews;

public sealed record GetReviewsResponse(
    long GameId, long? SteamAppId, int? NumReviews, int? ReviewScore,
    string? ReviewScoreDesc, int? TotalPositive, int? TotalNegative, int? TotalReviews);
