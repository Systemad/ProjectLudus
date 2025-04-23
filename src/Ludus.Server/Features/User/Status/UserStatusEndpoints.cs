using System.Security.Claims;
using Ludus.Server.Features.Games.Queries;
using Ludus.Server.Features.User.Status.Requests;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.User.Status;

public static class UserStatusEndpoints
{
    public static RouteGroupBuilder MapUserStatusEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/user/status").WithTags("User").WithOpenApi();
        group.MapGet("/", GetMyGameStatusesAsync).RequireAuthorization();
        group.MapPut("/{gameId:long}", UpdateGameStatusAsync).RequireAuthorization();
        group.MapGet("/{userId:guid}", GetGameStatusesAsync).RequireAuthorization("Admin");

        return group;
    }

    private static async Task<
        Results<Ok<List<UserGameStatusDto>>, UnauthorizedHttpResult>
    > GetMyGameStatusesAsync(IUserStore db, ClaimsPrincipal user)
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
                .Select(s => new UserGameStatusDto
                {
                    Id = s.Id,
                    //UserId = s.UserId,
                    Game = gameDict.GetValueOrDefault(s.GameId),
                    Status = s.Status,
                })
                .ToList();
            return TypedResults.Ok(dtos);
        }

        return TypedResults.Unauthorized();
    }

    private static async Task<
        Results<Ok<List<UserGameStatusDto>>, BadRequest>
    > GetGameStatusesAsync(IUserStore db, Guid userId)
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
            .Select(s => new UserGameStatusDto
            {
                Id = s.Id,
                //UserId = s.UserId,
                Game = gameDict.GetValueOrDefault(s.GameId),
                Status = s.Status,
            })
            .ToList();
        return TypedResults.Ok(dtos);
    }

    private static async Task<
        Results<Ok<List<UserGameStatusDto>>, BadRequest>
    > UpdateGameStatusAsync(
        IUserStore db,
        ClaimsPrincipal user,
        long gameId,
        [FromBody] UpdateGameStatusRequest status
    )
    {
        await using var session = db.LightweightSession();

        var userId = Guid.Parse(user.Identity.Name);
        var gameStatus = await session
            .Query<UserGameStatus>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == status.GameId);

        if (gameStatus is null)
        {
            gameStatus = new UserGameStatus
            {
                UserId = userId,
                GameId = status.GameId,
                Status = status.GameStatus,
            };
        }
        else
        {
            gameStatus.Status = status.GameStatus;
        }
        session.Store(gameStatus);
        await session.SaveChangesAsync();

        var gameStatuses = await session
            .Query<UserGameStatus>()
            .Where(x => x.UserId == userId)
            .ToListAsync();
        var gameIds = gameStatuses.Select(x => x.GameId).ToList();
        var games = await session.QueryAsync(new GetGamesByIdQuery(gameIds));
        var gameDict = games.ToDictionary(g => g.Id, g => g);
        var dtos = gameStatuses
            .Select(s => new UserGameStatusDto
            {
                Id = s.Id,
                //UserId = s.UserId,
                Game = gameDict.GetValueOrDefault(s.GameId),
                Status = s.Status,
            })
            .ToList();

        return TypedResults.Ok(dtos);
    }
}
