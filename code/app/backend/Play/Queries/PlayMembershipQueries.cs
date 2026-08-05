using Microsoft.EntityFrameworkCore;
using Play.Infrastructure.Persistence;

namespace Play.Queries;

public sealed class PlayMembershipQueries(PlayDbContext db)
{
    public async Task<IReadOnlyDictionary<long, GameMembership>> GetAsync(
        Guid userId,
        IReadOnlyCollection<long> gameIds,
        CancellationToken ct
    )
    {
        if (gameIds.Count == 0)
            return new Dictionary<long, GameMembership>();

        var memberships = await db
            .ListItems.AsNoTracking()
            .Where(item => gameIds.Contains(item.GameId) && item.List.UserId == userId)
            .Select(item => new
            {
                item.GameId,
                item.ListId,
                item.List.IsDefault,
            })
            .ToListAsync(ct);

        return gameIds
            .Distinct()
            .ToDictionary(
                gameId => gameId,
                gameId =>
                {
                    var entries = memberships.Where(item => item.GameId == gameId).ToList();
                    return new GameMembership(
                        entries.Any(item => item.IsDefault),
                        entries.Select(item => item.ListId).ToList()
                    );
                }
            );
    }
}

public sealed record GameMembership(bool IsWishlisted, IReadOnlyList<Guid> ListIds);
