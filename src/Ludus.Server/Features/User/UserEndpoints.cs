using System.Security.Claims;
using Ludus.Shared.Features.Games;
using Ludus.Shared.Features.User;
using Ludus.Shared.Features.User.DTOs;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.User;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/user").WithTags("User").WithOpenApi();
        group.MapGet("me", MeAsync);
        group.MapPost("game/status", UpdateGameStatusAsync);

        return group;
    }

    private static async Task<
        Results<Ok<Shared.Features.User.User>, UnauthorizedHttpResult>
    > MeAsync(IUserStore db, ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated ?? false)
        {
            await using var session = db.QuerySession();
            var ludusUser = await session.LoadAsync<Shared.Features.User.User>(
                int.Parse(user.Identity.Name)
            );
            return TypedResults.Ok(ludusUser);
        }

        return TypedResults.Unauthorized();
    }

    private static async Task<IResult> UpdateGameStatusAsync(
        IUserStore db,
        ClaimsPrincipal user,
        [FromBody] UpdateGameStatus status
    )
    {
        await using var session = db.LightweightSession();

        int userId = int.Parse(user.Identity.Name);

        var dbStatus = await session
            .Query<UserGameStatus>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == status.GameId);

        if (dbStatus is null)
        {
            var userGameStatus = new UserGameStatus
            {
                UserId = userId,
                GameId = status.GameId,
                Status = status.GameStatus,
            };

            session.Store(userGameStatus);
        }
        else
        {
            dbStatus.Status = status.GameStatus;
        }

        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
