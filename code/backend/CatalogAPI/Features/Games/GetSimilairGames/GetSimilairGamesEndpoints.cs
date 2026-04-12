using CatalogAPI.Context;
using CatalogAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Games.GetSimilairGames;

public static class GetSimilairGamesEndpoints
{
    private record GetSimilairGamesResponse(List<GamesSearch> Games);

    public static IEndpointRouteBuilder MapGetSimilairGamesEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        var group = routeBuilder.MapGroup("/api/games").CacheOutput("DefaultCache");

        group
            .MapGet("/{gameId:long}/similar-games", GetSimilarGamesAsync)
            .Produces<GetSimilairGamesResponse>(StatusCodes.Status200OK);

        return routeBuilder;
    }

    private static async Task<IResult> GetSimilarGamesAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var similarGames = await db
            .Games.Where(g => g.Id == gameId)
            .SelectMany(g => g.SimilarGames)
            .Join(
                db.GamesSearches,
                similar => similar.Id,
                search => search.Id,
                (_, search) => search
            )
            .ToListAsync(cancellationToken);

        return Results.Ok(new GetSimilairGamesResponse(similarGames));
    }
}
