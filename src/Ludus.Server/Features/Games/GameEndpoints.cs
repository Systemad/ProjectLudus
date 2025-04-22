using Ludus.Server.Features.Games.Queries;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games;

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGameEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/games").WithTags("Games"); //.WithOpenApi();

        group.MapPost("/", GetGamesByIds);
        group.MapGet("/{id:long}", GetGameById);
        group.MapGet("/top", GetRatedGames);
        return group;
    }

    private static async Task<Results<Ok<List<Game>>, BadRequest<string>>> GetGamesByIds(
        [FromServices] IQuerySession session,
        [FromBody] List<long> ids
    )
    {
        if (ids.Count == 0)
            return TypedResults.BadRequest("You need to provide at least one ID");

        var results = await session.LoadManyAsync<Game>(ids);
        return TypedResults.Ok(results.ToList());
    }

    private static async Task<Results<Ok<Game>, NotFound>> GetGameById(
        [FromServices] IQuerySession session,
        long id
    )
    {
        var game = await session.Query<Game>().Where(g => g.Id == id).FirstOrDefaultAsync();
        if (game is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(game);
    }

    private static async Task<Ok<List<Game>>> GetRatedGames(
        [FromServices] IQuerySession session,
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null
    )
    {
        var pageQuery = new GamesPaginationQuery(pageNumber, 20);
        /*
         *         var games = await session
            .Query<Game>()
            .OrderByDescending(g => g.RatingCount)
            .ToPagedListAsync(pageNumber, pageSize);

         */
        var gameModes = await session.Query<GameMode>().ToListAsync(); // .FirstOrDefaultAsync(x => x.Id == 2);

        var games = await session.QueryAsync(pageQuery); //.OrderByDescending(g => g.RatingCount).ToList();
        //.ToPagedListAsync(pageNumber, pageSize);

        var gamesList = games.ToList();
        return TypedResults.Ok(gamesList);
    }
}
