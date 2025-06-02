using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

// TODO: FIX
public record GetGamesByIdsResult(IEnumerable<Game> Games);

public static class GetGamesByIdsAsync
{
    public static async Task<Results<Ok<GetGamesByIdsResult>, ProblemHttpResult>> Handler(
        [FromServices] IGameStore store,
        [FromBody] List<long> ids
    )
    {
        await using var session = store.QuerySession();
        if (ids.Count == 0)
            return TypedResults.Problem(
                type: "Bad Request",
                title: "No IDS",
                detail: "No IDs were provided",
                statusCode: StatusCodes.Status400BadRequest
            );

        var results = await session.LoadManyAsync<Game>(ids);
        return TypedResults.Ok(new GetGamesByIdsResult(results));
    }
}
