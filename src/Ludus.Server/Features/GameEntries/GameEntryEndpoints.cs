using Ludus.Server.Features.GameEntries.Handlers;

namespace Ludus.Server.Features.GameEntries;

public static class GameEntryEndpoints
{
    public static RouteGroupBuilder MapGameEntryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/status").WithTags("Status").WithOpenApi();
        group.MapGet("/", GetGameEntryPreviewAsync.Handler).RequireAuthorization();
        group
            .MapPut("/{gameId:long}", CreateOrUpdateGameStatusAsync.Handler)
            .RequireAuthorization();
        group
            .MapGet("/{userId:guid}", GetGameEntryDetailAsync.Handler)
            .RequireAuthorization("Admin");

        return group;
    }
}
