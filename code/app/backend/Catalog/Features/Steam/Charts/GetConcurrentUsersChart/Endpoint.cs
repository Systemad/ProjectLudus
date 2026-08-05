using Microsoft.AspNetCore.Mvc;

namespace Catalog.Features.Steam.Charts.GetConcurrentUsersChart;

public static class Endpoint
{
    public static async Task<IResult> HandleChartAsync(
        long gameId,
        [FromQuery] string? range,
        ISteamService steamService,
        CancellationToken ct
    )
    {
        var result = await steamService.GetConcurrentUsersChartAsync(gameId, range, ct);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}
