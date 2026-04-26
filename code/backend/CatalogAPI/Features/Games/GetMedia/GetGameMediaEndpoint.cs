using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Games.GetMedia;

public static class GetGameMediaEndpoint
{
    public record Response(GameMediaDto Game);

    public static async Task<IResult> HandleAsync(
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

        return Results.Ok(new Response(gameMediaDto));
    }
}
