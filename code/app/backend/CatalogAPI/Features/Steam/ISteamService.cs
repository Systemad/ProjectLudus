using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.Steam.Charts.GetChart;
using CatalogAPI.Features.Steam.Charts.GetConcurrentUsersChart;
using CatalogAPI.Features.Steam.Store.GetPricing;
using CatalogAPI.Features.Steam.Store.GetReviews;

namespace CatalogAPI.Features.Steam;

public interface ISteamService
{
    Task<GetPricingResponse?> GetPricingAsync(long gameId, CancellationToken ct);
    Task<GetReviewsResponse?> GetReviewsAsync(long gameId, CancellationToken ct);
    Task<List<GameBrowseDto>> GetChartAsync(Request request, CancellationToken ct);
    Task<ConcurrentUsersChartResponse?> GetConcurrentUsersChartAsync(long gameId, string? range, CancellationToken ct);
}
