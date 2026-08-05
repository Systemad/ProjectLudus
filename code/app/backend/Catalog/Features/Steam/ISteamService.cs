using Catalog.Features.Games.Common.Pagination;
using Catalog.Features.Steam.Charts.GetChart;
using Catalog.Features.Steam.Charts.GetConcurrentUsersChart;
using Catalog.Features.Steam.Store.GetPricing;
using Catalog.Features.Steam.Store.GetReviews;

namespace Catalog.Features.Steam;

public interface ISteamService
{
    Task<GetPricingResponse?> GetPricingAsync(long gameId, CancellationToken ct);
    Task<GetReviewsResponse?> GetReviewsAsync(long gameId, CancellationToken ct);
    Task<PagedGamesResponse> GetChartAsync(Request request, CancellationToken ct);
    Task<ConcurrentUsersChartResponse?> GetConcurrentUsersChartAsync(
        long gameId,
        string? range,
        CancellationToken ct
    );
}
