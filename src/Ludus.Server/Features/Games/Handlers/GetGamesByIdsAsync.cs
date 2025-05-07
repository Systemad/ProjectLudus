using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public record GetGamesByIdsResult(IEnumerable<Game> Games);

public static class GetGamesByIdsAsync
{
    public static async Task<Results<Ok<GetGamesByIdsResult>, BadRequest<string>>> Handler(
        [FromServices] IGameStore store,
        [FromBody] List<long> ids
    )
    {
        await using var session = store.QuerySession();
        if (ids.Count == 0)
            return TypedResults.BadRequest("You need to provide at least one ID");

        var results = await session.LoadManyAsync<Game>(ids);
        return TypedResults.Ok(new GetGamesByIdsResult(results));
    }
}
