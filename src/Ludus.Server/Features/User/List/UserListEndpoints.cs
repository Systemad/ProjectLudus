using System.Security.Claims;
using Ludus.Server.Features.Games.Queries;
using Ludus.Server.Features.User.DTO;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.User.List;

public static class UserListEndpoints
{
    public static RouteGroupBuilder MapUserListEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("api/user/list")
            .WithTags("User", "list")
            .WithOpenApi()
            .RequireAuthorization();
        group.MapGet("/", GetMyUserGameListAsync);
        group.MapPost("/", CreateUserGameListAsync);

        group.MapGet("/{id:guid}", GetGamesOfUserListAsync);
        group.MapPut("/{id:guid}", UpdateUserGameListAsync);
        group.MapDelete("/{id:guid}", RemoveGameFromUserGameListAsync);
        group.MapPost("/{id:guid}", AddGameFromUserGameListAsync);
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
                    Count = s.GamesIds.Count,
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

    private static async Task<Created<UserGameListDto>> CreateUserGameListAsync(
        IUserStore db,
        ClaimsPrincipal user,
        Guid id,
        [FromBody] CreateOrUpdateGameListDto item
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var userGameList = new UserGameList
        {
            UserId = userId,
            Name = item.Name,
            Public = item.Public,
        };
        session.Store(userGameList);
        await session.SaveChangesAsync();
        var newItem = await session.LoadAsync<UserGameList>(userGameList.Id);
        return TypedResults.Created($"/user/list/{newItem.Id}", newItem.AsUserGameListDto());
    }

    private static async Task<Created<UserGameListDto>> UpdateUserGameListAsync(
        IUserStore db,
        ClaimsPrincipal user,
        Guid id,
        [FromBody] CreateOrUpdateGameListDto item
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == id);
        updateList.Id = item.Id;
        updateList.Name = item.Name;
        updateList.Public = item.Public;
        session.Store(updateList);
        await session.SaveChangesAsync();
        var newItem = await session.LoadAsync<UserGameList>(updateList.Id);
        return TypedResults.Created($"/user/list/{newItem.Id}", newItem.AsUserGameListDto());
    }

    private static async Task<Results<NotFound<string>, Ok>> RemoveGameFromUserGameListAsync(
        IUserStore db,
        ClaimsPrincipal user,
        Guid id,
        [FromBody] AddOrRemoveGameDto game
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == id);
        var removed = updateList.GamesIds.Remove(game.GameId);
        if (!removed)
            return TypedResults.NotFound("Game is not in the list");
        session.Store(updateList);
        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, BadRequest<string>>> AddGameFromUserGameListAsync(
        IUserStore db,
        ClaimsPrincipal user,
        Guid id,
        [FromBody] AddOrRemoveGameDto game
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (updateList.GamesIds.Contains(game.GameId))
            return TypedResults.BadRequest("Game is already in the list");

        updateList.GamesIds.Add(game.GameId);
        session.Store(updateList);
        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
