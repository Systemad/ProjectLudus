using Catalog.Features;

namespace Catalog.Queries;

public sealed class CatalogGameQueries(AppDbContext db)
{
    public async Task<IReadOnlyDictionary<long, GameCard>> GetGameCardsAsync(
        IReadOnlyCollection<long> gameIds,
        CancellationToken ct
    )
    {
        if (gameIds.Count == 0)
            return new Dictionary<long, GameCard>();

        var cards = await db
            .Games.AsNoTracking()
            .Where(game => gameIds.Contains(game.Id))
            .Select(game => new
            {
                game.Id,
                game.Name,
                CoverImageId = game.CoverNavigation == null ? null : game.CoverNavigation.ImageId,
                game.FirstReleaseDateUtc,
                GameTypeName = game.GameTypeNavigation == null ? null : game.GameTypeNavigation.Type,
            })
            .ToListAsync(ct);

        return cards.ToDictionary(
            card => card.Id,
            card => new GameCard(
                ApiId.Format(card.Id),
                card.Name,
                card.CoverImageId,
                card.FirstReleaseDateUtc,
                card.GameTypeName
            )
        );
    }
}

public sealed record GameCard(
    string Id,
    string Name,
    string? CoverImageId,
    DateTime? FirstReleaseDateUtc,
    string? GameTypeName
);
