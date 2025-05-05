using System.Security.Claims;
using Ludus.Server.Features.Lists.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.GetList;

public static class GetListAsync
{
    public static async Task<Results<Ok<UserGameListDto>, BadRequest<string>>> Handle(
        UserListService listService,
        ClaimsPrincipal user,
        Guid listId,
        [FromQuery] bool preview = false
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        var listPreview = await listService.GetUserGameListDtoAsync(userId, listId, preview);
        return TypedResults.Ok(listPreview);
    }
}
