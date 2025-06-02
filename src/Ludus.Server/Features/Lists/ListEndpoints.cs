using Ludus.Server.Features.Lists.AddGameToList;
using Ludus.Server.Features.Lists.CreateList;
using Ludus.Server.Features.Lists.GetList;
using Ludus.Server.Features.Lists.RemoveGameFromList;
using Ludus.Server.Features.Lists.UpdateList;

namespace Ludus.Server.Features.Lists;

public static class UserListEndpoints
{
    public static RouteGroupBuilder MapListEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("api/list")
            .WithTags("List")
            .WithOpenApi()
            .RequireAuthorization();
        group.MapGet("/", GetListsAsync.Handle);
        group.MapPost("/", CreateListAsync.Handle);

        group.MapGet("/{listId:guid}", GetListAsync.Handle);
        group.MapPut("/{listId:guid}", UpdateListAsync.Handler);

        group.MapPost("/{listId:guid}/add", AddGameToListAsync.Handler);
        group.MapDelete("/{listId:guid}/delete/{gameId:long}", RemoveGameFromListAsync.Handle);
        return group;
    }
}
