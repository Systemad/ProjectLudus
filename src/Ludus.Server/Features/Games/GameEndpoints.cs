using Ludus.Data;
using Ludus.Shared;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games;

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGameEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/games").WithTags("Games").WithOpenApi();

        group.MapPost("/", GetGamesByIds);
        group.MapGet("/{id:long}", GetGamesById);

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
}
