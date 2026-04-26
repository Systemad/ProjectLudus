using CatalogAPI.Features.Games.Common.Projections;

namespace CatalogAPI.Features.Games.GetSimilarGames;

public static class GetSimilarGamesEndpoint
{
    public record Response(List<GameDto> Games);

    public static async Task<IResult> HandleAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var similarGames = await db
            .Games.Where(g => g.Id == gameId)
            .SelectMany(g => g.SimilarGames)
            .Join(db.Games, similar => similar.Id, game => game.Id, (_, game) => game)
            .Where(game => game.FirstReleaseDateUtc.HasValue)
            .Select(GameDtoProjection.AsGameDto)
            .ToListAsync(cancellationToken);

        return Results.Ok(new Response(similarGames));
    }
}
