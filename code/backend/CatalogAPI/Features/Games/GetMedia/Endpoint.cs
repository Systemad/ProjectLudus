using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Games.GetMedia;

public static class Endpoint
{
    public record GetGameMediaResponse(GameMediaDto Game);

    public static RouteHandlerBuilder MapGetGameMediaEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        return routeBuilder
            .MapGet("/{gameId:long}/media", GetGameMediaAsync)
            .WithName($"{EndpointMetadata.Games}/GetMedia")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGameMediaResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
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
