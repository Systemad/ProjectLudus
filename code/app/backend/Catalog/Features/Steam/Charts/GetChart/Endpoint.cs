using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Steam.Charts.GetChart;

public static class Endpoint
{
    public static async Task<Ok<PagedGamesResponse>> HandleAsync(
        [AsParameters] Request request,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        var games = await steamService.GetChartAsync(request, cancellationToken);
        return TypedResults.Ok(games);
    }
}
