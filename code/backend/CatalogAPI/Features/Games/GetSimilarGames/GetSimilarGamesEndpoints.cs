using CatalogAPI.Context;
using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.Games.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Games.GetSimilarGames;

public static class GetSimilarGamesEndpoints
{
    private record GetSimilarGamesResponse(List<GamesSearchDto> Games);

    public static IEndpointRouteBuilder MapGetSimilarGamesEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        routeBuilder
            .MapGet("/{gameId:long}/similar-games", GetSimilarGamesAsync)
            .Produces<GetSimilarGamesResponse>(StatusCodes.Status200OK);

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
        var mapper = new GameSearchMapper();
       var hey = mapper.MapToDto(similarGames):
        return Results.Ok(new GetSimilarGamesResponse(mapper.MapToDto(similarGames)));
    }
}
