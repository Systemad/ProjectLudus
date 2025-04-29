using Ludus.Server.Features.Lists.Handlers;

namespace Ludus.Server.Features.Lists;

public static class UserListEndpoints
{
    public static RouteGroupBuilder MapListEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("api/user/list")
            .WithTags("List")
            .WithOpenApi()
            .RequireAuthorization();
        group.MapGet("/", GetMyListsAsync.Handle);
        group.MapPost("/", CreateListAsync.Handle);

        group.MapGet("/{listId:guid}", GetListGamesAsync.Handle);
        group.MapPut("/{listId:guid}", UpdateListAsync.Handle);

        group.MapPost("/{listId:guid}/add", AddGameToListAsync.Handle);
        group.MapDelete("/{listId:guid}/delete", RemoveGameListAsync.Handle);
        return group;
    }
}
