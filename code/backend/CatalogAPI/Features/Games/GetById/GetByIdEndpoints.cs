using CatalogAPI.Context;
using CatalogAPI.Data;
using CatalogAPI.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Games.GetById;

public static class GetByIdEndpoints
{
    public record GetGameByIdResponse(GameDto Game);

    public static IEndpointRouteBuilder MapGetByIdEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/games");

        group
            .MapGet("/{gameId:long}", GetGameByIdAsync)
            .Produces<GetGameByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return routeBuilder;
    }

    private static async Task<IResult> GetGameByIdAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var game = await db
            .Games.Include(g => g.CoverNavigation)
            .Include(g => g.GameTypeNavigation)
            .Include(g => g.GameStatusNavigation)
            .Include(g => g.Platforms)
            .Include(g => g.Genres)
            .Include(g => g.Themes)
            .Include(g => g.GameModes)
            .Include(g => g.PlayerPerspectives)
            .Include(g => g.ReleaseDates)
            .Include(g => g.Websites)
            .Include(g => g.Videos)
            .Include(g => g.Screenshots)
            .Include(g => g.AlternativeNames)
            .Include(g => g.ExternalGames)
            .AsSplitQuery()
            .SingleOrDefaultAsync(g => g.Id == gameId, cancellationToken);

        if (game is null)
            return Results.NotFound();

        return Results.Ok(new GetGameByIdResponse(GameDto.From(game)));
    }
}
