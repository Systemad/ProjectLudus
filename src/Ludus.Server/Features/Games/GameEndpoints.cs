using Ludus.Shared;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games;

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGameEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/games").WithTags("Games").WithOpenApi();

        group.MapPost("/", GetGamesByIds);
        group.MapPost("/{id:long}", GetGamesById);
        group.MapGet("/top", GetRatedGames);
        return group;
    }

    private static async Task<Ok<List<Game>>> GetGamesByIds(
        [FromServices] IQuerySession session,
        [FromBody] List<long> ids
    )
    {
        var results = await session.LoadManyAsync<Game>(ids);
        return TypedResults.Ok(results.ToList());
    }

    private static async Task<Results<Ok<Game>, NotFound>> GetGamesById(
        [FromServices] IQuerySession session,
        long id
    )
    {
        var game = await session.Query<Game>().Where(g => g.Id == id).FirstOrDefaultAsync();
        if (game is null)
            TypedResults.NotFound();

        return TypedResults.Ok(game);
    }

    private static async Task<Results<Ok<List<Game>>, NotFound>> GetRatedGames(
        [FromServices] IQuerySession session,
        int pageNumber = 1,
        int pageSize = 20
    )
    {
        var games = await session
            .Query<Game>()
            .OrderByDescending(g => g.RatingCount)
            .ToPagedListAsync(pageNumber, pageSize);
        if (!games.Any())
            TypedResults.NotFound();

        return TypedResults.Ok(games.ToList());
    }
}
