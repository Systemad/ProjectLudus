namespace CatalogAPI.Features.IGDB.GetPopscore;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        [AsParameters] GetPopscoreQuery request,
        IIGDBService igdbService,
        CancellationToken cancellationToken
    )
    {
        var result = await igdbService.GetPopscoreAsync(request.PopularityTypeId, request.Limit, cancellationToken);
        return Results.Ok(result);
    }
}
