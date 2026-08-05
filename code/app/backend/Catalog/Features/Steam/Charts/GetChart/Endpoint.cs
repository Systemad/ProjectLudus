namespace Catalog.Features.Steam.Charts.GetChart;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        [AsParameters] Request request,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        var games = await steamService.GetChartAsync(request, cancellationToken);
        return Results.Ok(games);
    }
}
