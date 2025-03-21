using System.Security.Claims;
using Ludus.Shared;
using Ludus.Shared.Features.Games;
using Ludus.Shared.Features.User;
using Ludus.Shared.Features.User.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    private static async Task<Results<Ok<LudusUser>, UnauthorizedHttpResult>> MeAsync(
        AppDbContext db,
        ClaimsPrincipal user
    )
    {
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var ludusUser = await db.Users.FirstOrDefaultAsync(u =>
                u.Id == int.Parse(user.Identity.Name)
            );
            return TypedResults.Ok(ludusUser);
        }

        return TypedResults.Unauthorized();
    }

    private static async Task<IResult> UpdateGameStatusAsync(
        AppDbContext db,
        ClaimsPrincipal user,
        [FromBody] UpdateGameStatus status
    )
    {
        int userId = int.Parse(user.Identity.Name);
        var dbStatus = await db.UserGameStatus.FirstOrDefaultAsync(ug =>
            ug.Id == userId && ug.GameId == status.GameId
        );
        if (dbStatus is not null)
        {
            dbStatus.Status = status.GameStatus;
        }
        else
        {
            db.UserGameStatus.Add(
                new UserGameStatus
                {
                    LudusUserId = userId,
                    GameId = status.GameId,
                    Status = status.GameStatus,
                }
            );
        }

        await db.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
