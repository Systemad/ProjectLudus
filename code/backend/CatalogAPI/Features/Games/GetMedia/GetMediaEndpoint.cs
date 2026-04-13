using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Games.GetMedia;

public static class GetMediaEndpoint
{
    public record GetGameMediaResponse(GameMediaDto Game);

    public static IEndpointRouteBuilder MapGetMediaEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("/{gameId:long}/media", GetGameMediaAsync)
            .Produces<GetGameMediaResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return routeBuilder;
    }

    private static async Task<IResult> GetGameMediaAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var gameMediaDto = await db
            .Games.Where(g => g.Id == gameId)
            .Select(g => new GameMediaDto
            {
                Screenshots = g
                    .Screenshots.Where(s => !string.IsNullOrEmpty(s.ImageId))
                    .Select(s => s.ImageId!)
                    .ToList(),
                Videos = g
                    .Videos.Select(v => new GameMediaVideoDto(
                        v.Name ?? string.Empty,
                        v.VideoId ?? string.Empty
                    ))
                    .ToList(),
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (gameMediaDto is null)
            return Results.NotFound();

        return Results.Ok(new GetGameMediaResponse(gameMediaDto));
    }
}
