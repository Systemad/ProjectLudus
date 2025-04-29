using Ludus.Server.Features.Games.Handlers;

namespace Ludus.Server.Features.Games;

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGameEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/games").WithTags("Games"); //.WithOpenApi();

        group.MapPost("/", GetGamesByIdsAsync.Handler);
        group.MapGet("/{id:long}", GetGameByIdAsync.Handler);
        group.MapGet("/top", GetTopRatedGamesAsync.Handler);
        group.MapGet("/search", GetGamesByParametersAsync.Handler);
        return group;
    }
}
