namespace CatalogAPI.Features.IGDB.GetStatistics;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        IIGDBService igdbService,
        CancellationToken cancellationToken
    )
    {
        var result = await igdbService.GetStatisticsAsync(cancellationToken);
        return Results.Ok(result);
    }
}
