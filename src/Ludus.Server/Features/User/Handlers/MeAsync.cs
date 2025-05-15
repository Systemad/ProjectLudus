using System.Security.Claims;
using Ludus.Server.Features.User.Models;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.User.Handlers;

public static class MeAsync
{
    public static async Task<Results<Ok<UserDto>, UnauthorizedHttpResult>> Handler(
        IDocumentStore db,
        ClaimsPrincipal user
    )
    {
        Console.WriteLine("Handler");

        if (user.Identity?.IsAuthenticated ?? false)
        {
            Console.WriteLine("if (user.Identity?.IsAuthenticated ?? false)");
            var userId = Guid.Parse(user.Identity.Name);
            await using var session = db.QuerySession();
            var ludusUser = await session.LoadAsync<Models.User>(userId);
            if (ludusUser is null)
            {
                return TypedResults.Unauthorized();
            }
            var dto = new UserDto
            {
                Id = ludusUser.Id,
                Name = ludusUser.Name,
                Role = ludusUser.Name,
                SteamId = ludusUser.SteamId,
                AvatarImageId = ludusUser.AvatarImageId,
                CreatedDate = ludusUser.CreatedDate,
                UserImage = ludusUser.UserImage,
            };
            return TypedResults.Ok(dto);
        }

        return TypedResults.Unauthorized();
    }
}
