using System.Security.Claims;
using Ludus.Server.Features.Lists.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.GetList;

public static class GetListsAsync
{
    public static async Task<Results<Ok<IEnumerable<UserGameListDto>>, BadRequest<string>>> Handle(
        UserListService listService,
        ClaimsPrincipal user,
        [FromQuery] bool preview = false
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        var listPreview = await listService.GetUserGameListsDtoAsync(userId, preview);
        return TypedResults.Ok(listPreview);
    }
}
