using Microsoft.AspNetCore.Mvc;

namespace Catalog.Features.Steam.Charts.GetConcurrentUsersChart;

public static class Endpoint
{
    public static async Task<IResult> HandleChartAsync(
        string gameId,
        [FromQuery] string? range,
        ISteamService steamService,
        CancellationToken ct
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return Results.BadRequest();

        var result = await steamService.GetConcurrentUsersChartAsync(parsedGameId, range, ct);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}
