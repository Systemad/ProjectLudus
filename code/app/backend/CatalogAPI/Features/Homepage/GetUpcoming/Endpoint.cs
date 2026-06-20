

namespace CatalogAPI.Features.Homepage.GetUpcoming;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        IHomepageService homepageService,
        CancellationToken cancellationToken
    )
    {
        var games = await homepageService.GetUpcomingAsync(cancellationToken);
        return Results.Ok(new UpcomingResponse(games));
    }
}