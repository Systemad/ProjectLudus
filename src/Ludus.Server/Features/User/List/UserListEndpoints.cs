using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.User.List;

public static class UserListEndpoints
{
    public static RouteGroupBuilder MapUserListEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/user/list").WithTags("User", "list").WithOpenApi();
        group.MapGet("/", GetMyUserGameListAsync).RequireAuthorization();
        group.MapGet("/{id:guid}", GetGamesOfUserListAsync).RequireAuthorization();

        return group;
    }

    private static async Task<
        Results<Ok<List<UserGameListDto>>, UnauthorizedHttpResult>
    > GetMyUserGameListAsync(IUserStore db, ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var userId = Guid.Parse(user.Identity.Name);
            await using var session = db.QuerySession();
            var lists = await session
                .Query<UserGameList>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
            var dtos = lists
                .Select(s => new UserGameListDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    //UserId = s.UserId,
                })
                .ToList();
            return TypedResults.Ok(dtos);
        }

        return TypedResults.Unauthorized();
    }

    private static async Task<
        Results<Ok<UserGameListDto>, BadRequest<string>>
    > GetGamesOfUserListAsync(IUserStore db, ClaimsPrincipal user, Guid id)
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.QuerySession();
        var list = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (list.GamesIds is null || list.GamesIds.Count <= 0)
        {
            return TypedResults.BadRequest("List is empty!");
        }

        var games = await session.QueryAsync(new GetGamesByIdQuery(list.GamesIds));

        var dto = new UserGameListDto
        {
            Id = list.Id,
            Name = list.Name,
            Public = list.Public,
            Games = games.ToList(),
        };
        return TypedResults.Ok(dto);
    }
}
