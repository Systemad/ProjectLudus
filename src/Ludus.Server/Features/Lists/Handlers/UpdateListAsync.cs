using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.Handlers;

public record UpdateListParameter(Guid Id, string Name, bool Public);

public record UpdateListResult(Guid Id, string Name, bool Public);

public static class UpdateListAsync
{
    public static async Task<Created<UpdateListResult>> Handle(
        IUserStore db,
        ClaimsPrincipal user,
        Guid listId,
        [FromBody] UpdateListParameter parameter
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);
        updateList.Id = parameter.Id;
        updateList.Name = parameter.Name;
        updateList.Public = parameter.Public;
        session.Store(updateList);
        await session.SaveChangesAsync();
        var newItem = await session.LoadAsync<UserGameList>(updateList.Id);
        var result = new UpdateListResult(newItem.Id, newItem.Name, newItem.Public);
        return TypedResults.Created($"/user/list/{newItem.Id}", result);
    }
}
