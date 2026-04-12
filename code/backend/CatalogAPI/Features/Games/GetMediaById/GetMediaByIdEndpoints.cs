using System.Linq;
using CatalogAPI.Context;
using CatalogAPI.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Games.GetMediaById;

public static class GetMediaByIdEndpoints
{
    public record GetGameMediaByIdResponse(GameMedia Game);

    public static IEndpointRouteBuilder MapGetMediaByIdEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        var group = routeBuilder.MapGroup("/api/games").CacheOutput("DefaultCache");

        group
            .MapGet("/{gameId:long}/media", GetGameMediaById)
            .Produces<GetGameMediaByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return routeBuilder;
    }

    private static async Task<IResult> GetGameMediaById(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var gameMediaDto = await db
            .Games.AsNoTracking()
            .Where(g => g.Id == gameId)
            .Select(g => new GameMedia
            {
                Screenshots = g
                    .Screenshots.Where(s => !string.IsNullOrEmpty(s.ImageId))
                    .Select(s => s.ImageId!)
                    .ToList(),
                Videos = g
                    .Videos.Select(v => new GameMediaVideo(
                        v.Name ?? string.Empty,
                        v.VideoId ?? string.Empty
                    ))
                    .ToList(),
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (gameMediaDto is null)
            return Results.NotFound();

        return Results.Ok(new GetGameMediaByIdResponse(gameMediaDto));
    }
}
