using Microsoft.EntityFrameworkCore;
using Play.Features.Lists.Common.Dtos;
using Play.Features.Lists.Create;
using Play.Features.Lists.Update;
using Play.Infrastructure.Persistence;

namespace Play.Features.Lists;

public sealed class ListService(PlayDbContext db)
{
    public Task<List<ListSummaryResponse>> GetSummariesAsync(Guid userId, CancellationToken ct) =>
        db
            .Lists.AsNoTracking()
            .Where(list => list.UserId == userId)
            .OrderByDescending(list => list.IsDefault)
            .ThenBy(list => list.Name)
            .Select(list => new ListSummaryResponse(
                list.Id,
                list.Name,
                list.Description,
                list.Visibility,
                list.IsDefault,
                list.Items.Count,
                list.CreatedAt,
                list.UpdatedAt
            ))
            .ToListAsync(ct);

    public Task<List<UserList>> GetMineAsync(Guid userId, CancellationToken ct) =>
        db
            .Lists.Include(x => x.Items)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

    public async Task<UserList?> GetAsync(
        Guid userId,
        Guid id,
        bool ownerOnly,
        CancellationToken ct
    )
    {
        var list = await db.Lists.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id, ct);
        return
            list is not null
            && (list.UserId == userId || (!ownerOnly && list.Visibility == ListVisibility.Public))
            ? list
            : null;
    }

    public async Task<UserList> CreateAsync(
        Guid userId,
        CreateListRequest request,
        CancellationToken ct
    )
    {
        var list = new UserList
        {
            UserId = userId,
            Name = request.Name.Trim(),
            Description = request.Description,
            Visibility = request.Visibility,
        };
        db.Lists.Add(list);
        await db.SaveChangesAsync(ct);
        return list;
    }

    public async Task<bool> UpdateAsync(
        Guid userId,
        Guid id,
        UpdateListRequest request,
        CancellationToken ct
    )
    {
        var list = await db
            .Lists.AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        if (list is null || list.IsDefault)
            return false;
        list.Name = request.Name.Trim();
        list.Description = request.Description;
        list.Visibility = request.Visibility;
        list.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken ct)
    {
        var list = await db
            .Lists.AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId && !x.IsDefault, ct);
        if (list is null)
            return false;
        db.Lists.Remove(list);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> SetGameAsync(
        Guid userId,
        Guid listId,
        long gameId,
        bool add,
        CancellationToken ct
    )
    {
        var list = await db
            .Lists.AsTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == listId && x.UserId == userId, ct);
        if (list is null)
            return false;
        var item = list.Items.FirstOrDefault(x => x.GameId == gameId);
        if (add && item is null)
        {
            list.Items.Add(new ListItem { ListId = list.Id, GameId = gameId });
            db.ListHistory.Add(
                new ListHistoryEntry
                {
                    ListId = list.Id,
                    UserId = userId,
                    GameId = gameId,
                    Action = ListAction.Added,
                }
            );
        }
        if (!add && item is not null)
        {
            db.ListItems.Remove(item);
            db.ListHistory.Add(
                new ListHistoryEntry
                {
                    ListId = list.Id,
                    UserId = userId,
                    GameId = gameId,
                    Action = ListAction.Removed,
                }
            );
        }
        list.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public Task<List<ListHistoryEntry>> GetHistoryAsync(
        Guid userId,
        Guid listId,
        CancellationToken ct
    ) =>
        db
            .ListHistory.Where(x => x.ListId == listId && x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);

    public async Task<PagedListGamesResponse?> GetGamesAsync(
        Guid userId,
        Guid listId,
        bool ownerOnly,
        int page,
        int pageSize,
        CancellationToken ct
    )
    {
        var list = await db
            .Lists.AsNoTracking()
            .Where(x => x.Id == listId)
            .Select(x => new { x.UserId, x.Visibility })
            .FirstOrDefaultAsync(ct);

        if (
            list is null
            || (ownerOnly && list.UserId != userId)
            || (!ownerOnly && list.UserId != userId && list.Visibility != ListVisibility.Public)
        )
            return null;

        var results = await db
            .ListItems.AsNoTracking()
            .Where(x => x.ListId == listId)
            .OrderByDescending(x => x.AddedAt)
            .ThenBy(x => x.GameId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize + 1)
            .Select(x => new ListGameResponse(x.GameId, x.AddedAt))
            .ToListAsync(ct);

        return PagedListGamesResponse.Create(results, page, pageSize);
    }

    public async Task<GameListMembershipResponse> GetMembershipAsync(
        Guid userId,
        long gameId,
        CancellationToken ct
    )
    {
        var memberships = await db
            .ListItems.AsNoTracking()
            .Where(x => x.GameId == gameId && x.List.UserId == userId)
            .Select(x => new { x.ListId, x.List.IsDefault })
            .ToListAsync(ct);

        return new GameListMembershipResponse(
            memberships.Select(x => x.ListId).ToList(),
            memberships.Any(x => x.IsDefault)
        );
    }
}
