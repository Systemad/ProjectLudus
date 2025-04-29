using System.Security.Claims;
using Ludus.Server.Features.Games;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.User.Status.Handlers;

public record UpdateGameStatusParameter(long GameId, GameStatus GameStatus);

public record UpdateGameStatusResult(Guid Id, Game Game, GameStatus Status);

public static class UpdateGameStatusAsync
{
    public static async Task<Results<Ok<UpdateGameStatusResult>, BadRequest>> Handler(
        IUserStore db,
        IDocumentStore gameStore,
        ClaimsPrincipal user,
        long gameId,
        [FromBody] UpdateGameStatusParameter parameter
    )
    {
        await using var session = db.LightweightSession();
        await using var gameSession = gameStore.QuerySession();
        var userId = Guid.Parse(user.Identity.Name);
        var gameStatus = await session
            .Query<UserGameStatus>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == parameter.GameId);

        if (gameStatus is null)
        {
            gameStatus = new UserGameStatus
            {
                UserId = userId,
                GameId = parameter.GameId,
                Status = parameter.GameStatus,
            };
        }
        else
        {
            gameStatus.Status = parameter.GameStatus;
        }
        session.Store(gameStatus);
        await session.SaveChangesAsync();

        var updatedStatus = await session.LoadAsync<UserGameStatus>(gameStatus.Id);
        var game = await gameSession.LoadAsync<Game>(updatedStatus.GameId);

        var result = new UpdateGameStatusResult(updatedStatus.Id, game, updatedStatus.Status);

        return TypedResults.Ok(result);
    }
}
