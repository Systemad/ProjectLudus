namespace Catalog.Features.Steam.Store.GetReviews;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        string gameId,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return Results.BadRequest();

        var reviews = await steamService.GetReviewsAsync(parsedGameId, cancellationToken);
        return reviews is null ? Results.NotFound() : Results.Ok(reviews);
    }
}
