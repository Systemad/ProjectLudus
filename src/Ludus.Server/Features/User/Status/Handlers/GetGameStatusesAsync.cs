using Ludus.Server.Features.Games;
using Ludus.Server.Features.Games.Queries;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.User.Status.Handlers;

public record GetGameStatusesResult(Guid Id, Game Game, GameStatus Status);

public static class GetGameStatusesAsync
{
    public static async Task<Results<Ok<List<GetGameStatusesResult>>, BadRequest>> Handler(
        IUserStore db,
        Guid userId
    )
    {
        await using var session = db.QuerySession();
        var gameStatuses = await session
            .Query<UserGameStatus>()
            .Where(x => x.UserId == userId)
            .ToListAsync();
        var gameIds = gameStatuses.Select(x => x.GameId).ToList();
        var games = await session.QueryAsync(new GetGamesByIdQuery(gameIds));

        var gameDict = games.ToDictionary(g => g.Id, g => g);
        var dtos = gameStatuses
            .Select(status => new GetGameStatusesResult(
                status.Id,
                gameDict.GetValueOrDefault(status.GameId),
                status.Status
            ))
            .ToList();
        return TypedResults.Ok(dtos);
    }
}
