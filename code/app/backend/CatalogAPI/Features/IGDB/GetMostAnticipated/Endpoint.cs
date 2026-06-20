
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Features.IGDB.GetMostAnticipated;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        [FromQuery] int? limit,
        IIGDBService igdbService,
        CancellationToken cancellationToken
    )
    {
        var games = await igdbService.GetMostAnticipatedAsync(limit, cancellationToken);
        return Results.Ok(new AnticipatedGamesResponse(games));
    }
}
