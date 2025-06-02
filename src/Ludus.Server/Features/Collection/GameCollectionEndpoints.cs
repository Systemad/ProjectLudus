using Ludus.Server.Features.Collection.Handlers;

namespace Ludus.Server.Features.Collection;

public static class GameEntryEndpoints
{
    public static RouteGroupBuilder MapGameCollectionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/collection").WithTags("Collection").WithOpenApi();

        group.MapGet("/", GetGameCollectionsAsync.Handler).RequireAuthorization();

        group.MapGet("/{gameId:long}", GetGameCollectionDetailAsync.Handler).RequireAuthorization();

        group.MapPut("/", CreateOrUpdateGameStatusAsync.Handler).RequireAuthorization();

        return group;
    }
}
