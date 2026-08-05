using Microsoft.EntityFrameworkCore;
using Play.Infrastructure.Persistence;

namespace Play.Queries;

public sealed class PlayListQueries(PlayDbContext db)
{
    public async Task<PlaySavedGamesPage?> GetAsync(
        Guid userId,
        Guid listId,
        SavedGameCursor? cursor,
        int pageSize,
        CancellationToken ct
    )
    {
        var list = await db
            .Lists.AsNoTracking()
            .Where(item => item.Id == listId && item.UserId == userId)
            .Select(item => new PlayList(item.Id, item.Name, item.Visibility, item.IsDefault))
            .FirstOrDefaultAsync(ct);

        if (list is null)
            return null;

        var query = db.ListItems.AsNoTracking().Where(item => item.ListId == listId);

        if (cursor is not null)
        {
            var beforeCursor = query.Where(item => item.AddedAt < cursor.AddedAt);
            var tiedCursor = query.Where(item =>
                item.AddedAt == cursor.AddedAt && item.GameId > cursor.GameId
            );
            query = beforeCursor.Concat(tiedCursor);
        }

        var results = await query
            .OrderByDescending(item => item.AddedAt)
            .ThenBy(item => item.GameId)
            .Take(pageSize + 1)
            .Select(item => new SavedGame(item.GameId, item.AddedAt))
            .ToListAsync(ct);

        return PlaySavedGamesPage.Create(list, results, pageSize);
    }
}

public sealed record PlayList(Guid Id, string Name, ListVisibility Visibility, bool IsDefault);

public sealed record SavedGame(long GameId, DateTimeOffset AddedAt);

public sealed record SavedGameCursor(DateTimeOffset AddedAt, long GameId);

public sealed record PlaySavedGamesPage(
    PlayList List,
    IReadOnlyList<SavedGame> Games,
    SavedGameCursor? NextCursor
)
{
    public static PlaySavedGamesPage Create(PlayList list, List<SavedGame> results, int pageSize)
    {
        var hasMore = results.Count > pageSize;
        var games = results.Take(pageSize).ToList();

        return new PlaySavedGamesPage(
            list,
            games,
            hasMore ? new SavedGameCursor(games[^1].AddedAt, games[^1].GameId) : null
        );
    }
}
