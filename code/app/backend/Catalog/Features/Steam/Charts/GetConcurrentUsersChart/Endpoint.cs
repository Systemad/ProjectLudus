using Microsoft.AspNetCore.Mvc;

namespace Catalog.Features.Steam.Charts.GetConcurrentUsersChart;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<ConcurrentUsersChartResponse>>> HandleChartAsync(
        string gameId,
        [FromQuery] string? range,
        ISteamService steamService,
        CancellationToken ct
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var result = await steamService.GetConcurrentUsersChartAsync(parsedGameId, range, ct);
        return result is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(result);
    }
}
