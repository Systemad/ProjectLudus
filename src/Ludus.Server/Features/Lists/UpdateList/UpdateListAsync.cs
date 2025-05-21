using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.UpdateList;

public record UpdateListQuery(Guid Id, string Name, bool Public);

public static class UpdateListAsync
{
    public static async Task<Created<UserGameList>> Handler(
        IDocumentStore db,
        ClaimsPrincipal user,
        Guid listId,
        [FromBody] UpdateListQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);
        updateList.Id = query.Id;
        updateList.Name = query.Name;
        updateList.Public = query.Public;
        session.Store(updateList);
        await session.SaveChangesAsync();
        return TypedResults.Created($"/list/{updateList.Id}", updateList);
    }
}
