using System.Security.Claims;
using Ludus.Server.Features.User.DTO;
using Ludus.Shared.Features.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.Handlers;

public record CreateListParameter(string Name, bool Public);

public record CreateListResult(Guid Id, string Name, bool Public, List<Game>? Games);

public static class CreateListAsync
{
    public static async Task<Created<CreateListResult>> Handle(
        IUserStore db,
        ClaimsPrincipal user,
        [FromBody] CreateListParameter parameter
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var userGameList = new UserGameList
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = parameter.Name,
            Public = parameter.Public,
        };
        session.Store(userGameList);
        await session.SaveChangesAsync();
        var newItem = await session.LoadAsync<UserGameList>(userGameList.Id);
        var result = new CreateListResult(
            newItem.Id,
            newItem.Name,
            newItem.Public,
            Games: new List<Game>()
        );
        return TypedResults.Created($"/user/list/{newItem.Id}", result);
    }
}
