using CatalogAPI.Context;
using CatalogAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.PopularityTypes.GetById;

public static class GetByIdEndpoints
{
    public record PopularityGamesResponse(List<GamesSearch> Games);

    public static IEndpointRouteBuilder MapGetByIdEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/popularity");

        group
            .MapGet("/{popularityTypeId:long}", GetPopularityRailAsync)
            .Produces<PopularityGamesResponse>(StatusCodes.Status200OK);

        return routeBuilder;
    }

    private static async Task<IResult> GetPopularityRailAsync(
        long popularityTypeId,
        int limit,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var topGameIds = await db
            .PopularityPrimitives.Where(p =>
                p.PopularityType == popularityTypeId && p.GameId.HasValue
            )
            .OrderByDescending(p => p.Value)
            .Select(p => p.GameId!.Value)
            .Take(limit)
            .ToListAsync(cancellationToken);

        if (topGameIds.Count == 0)
            return Results.Ok(new PopularityGamesResponse([]));

        var gamesDict = await db
            .GamesSearches
            .Where(g => g.Id.HasValue && topGameIds.Contains(g.Id.Value))
            .Where(x => x.GameType == "Main Game")
            .ToDictionaryAsync(g => g.Id!.Value, cancellationToken);

        var orderedGames = topGameIds
            .Distinct()
            .Where(gamesDict.ContainsKey)
            .Select(id => gamesDict[id])
            .ToList();

        return Results.Ok(new PopularityGamesResponse(orderedGames));
    }
}
