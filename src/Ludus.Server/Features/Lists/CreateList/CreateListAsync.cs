using System.Security.Claims;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.CreateList;

public record CreateListQuery(string Name, bool Public);

public static class CreateListAsync
{
    public static async Task<Created<UserGameListDto>> Handle(
        IDocumentStore db,
        ClaimsPrincipal user,
        [FromBody] CreateListQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var userGameList = new UserGameList
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = query.Name,
            Public = query.Public,
        };
        session.Store(userGameList);
        await session.SaveChangesAsync();
        var newItem = await session.LoadAsync<UserGameList>(userGameList.Id);
        var result = new UserGameListDto(
            newItem.Id,
            newItem.Name,
            newItem.Public,
            new List<GameCollectionPreviewDto>()
        );
        return TypedResults.Created($"/user/list/{newItem.Id}", result);
    }
}
