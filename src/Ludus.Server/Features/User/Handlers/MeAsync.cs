using System.Security.Claims;
using Ludus.Server.Features.User.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.User.Handlers;

public static class MeAsync
{
    public static async Task<Results<Ok<Models.User>, UnauthorizedHttpResult>> Handler(
        IUserStore db,
        ClaimsPrincipal user
    )
    {
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var userId = Guid.Parse(user.Identity.Name);
            await using var session = db.QuerySession();
            var ludusUser = await session.QueryAsync(new GetUserByIdQuery() { Id = userId });
            return TypedResults.Ok(ludusUser);
        }

        return TypedResults.Unauthorized();
    }
}
