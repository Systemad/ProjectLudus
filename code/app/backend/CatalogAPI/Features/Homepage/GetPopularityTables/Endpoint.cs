namespace CatalogAPI.Features.Homepage.GetPopularityTables;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        IHomepageService homepageService,
        CancellationToken cancellationToken)
    {
        var result = await homepageService.GetPopularityTablesAsync(cancellationToken);
        return Results.Ok(result);
    }
}