using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.User;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/user").WithTags("User").WithOpenApi();
        group.MapGet("me", MeAsync);

        return group;
    }

    private static async Task<Results<Ok<User>, UnauthorizedHttpResult>> MeAsync(
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
