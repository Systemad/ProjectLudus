using System.Security.Claims;
using Ludus.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Server.Features.User;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/user").WithTags("User").WithOpenApi();
        group.MapGet("me", MeAsync);

        return group;
    }

    private static async Task<Results<Ok<LudusUser>, UnauthorizedHttpResult>> MeAsync(ClaimsPrincipal User)
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var db = new AppDbContext();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(User.Identity.Name));
            return TypedResults.Ok(user);
        }

        return TypedResults.Unauthorized();
    }
}