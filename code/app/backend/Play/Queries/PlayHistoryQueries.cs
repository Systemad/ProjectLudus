using Microsoft.EntityFrameworkCore;
using Play.Infrastructure.Persistence;

namespace Play.Queries;

public sealed class PlayHistoryQueries(PlayDbContext db)
{
    public async Task<PlayHistoryPage> GetAsync(
        Guid userId,
        HistoryCursor? cursor,
        int pageSize,
        CancellationToken ct
    )
    {
        var query = db.ListHistory.AsNoTracking().Where(entry => entry.UserId == userId);

        if (cursor is not null)
        {
            var beforeCursor = query.Where(entry => entry.CreatedAt < cursor.CreatedAt);
            var tiedCursor = query.Where(entry =>
                entry.CreatedAt == cursor.CreatedAt && entry.Id < cursor.Id
            );
            query = beforeCursor.Concat(tiedCursor);
        }

        var results = await query
            .OrderByDescending(entry => entry.CreatedAt)
            .ThenByDescending(entry => entry.Id)
            .Take(pageSize + 1)
            .Select(entry => new UserHistoryItem(
                entry.Id,
                entry.ListId,
                entry.GameId,
                entry.Action,
                entry.CreatedAt
            ))
            .ToListAsync(ct);

        return PlayHistoryPage.Create(results, pageSize);
    }
}

public sealed record UserHistoryItem(
    long Id,
    Guid ListId,
    long GameId,
    ListAction Action,
    DateTimeOffset CreatedAt
);

public sealed record HistoryCursor(DateTimeOffset CreatedAt, long Id);

public sealed record PlayHistoryPage(
    IReadOnlyList<UserHistoryItem> Items,
    HistoryCursor? NextCursor
)
{
    public static PlayHistoryPage Create(List<UserHistoryItem> results, int pageSize)
    {
        var hasMore = results.Count > pageSize;
        var items = results.Take(pageSize).ToList();

        return new PlayHistoryPage(
            items,
            hasMore ? new HistoryCursor(items[^1].CreatedAt, items[^1].Id) : null
        );
    }
}
