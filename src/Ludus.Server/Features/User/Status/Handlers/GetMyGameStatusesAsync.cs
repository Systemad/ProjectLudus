using System.Security.Claims;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.Games.Queries;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.User.Status.Handlers;

public record GetMyGameStatusesResult(Guid Id, Game Game, GameStatus Status);

public static class GetMyGameStatusesAsync
{
    public static async Task<
        Results<Ok<List<GetMyGameStatusesResult>>, UnauthorizedHttpResult>
    > Handler(IUserStore db, ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var userId = Guid.Parse(user.Identity.Name);
            await using var session = db.QuerySession();
            var gameStatuses = await session
                .Query<UserGameStatus>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
            var gameIds = gameStatuses.Select(x => x.GameId).ToList();
            var games = await session.QueryAsync(new GetGamesByIdQuery(gameIds));

            var gameDict = games.ToDictionary(g => g.Id, g => g);
            var dtos = gameStatuses
                .Select(s => new GetMyGameStatusesResult(
                    s.Id,
                    gameDict.GetValueOrDefault(s.GameId),
                    s.Status
                ))
                .ToList();
            return TypedResults.Ok(dtos);
        }

        return TypedResults.Unauthorized();
    }
}
