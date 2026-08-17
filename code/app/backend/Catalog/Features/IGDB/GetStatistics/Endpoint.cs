namespace Catalog.Features.IGDB.GetStatistics;

public static class Endpoint
{
    public static async Task<Ok<StatisticsResponse>> HandleAsync(
        IIGDBService igdbService,
        CancellationToken cancellationToken
    )
    {
        var result = await igdbService.GetStatisticsAsync(cancellationToken);
        return TypedResults.Ok(result);
    }
}
