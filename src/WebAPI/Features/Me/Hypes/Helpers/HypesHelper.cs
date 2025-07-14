using Microsoft.EntityFrameworkCore;
using WebAPI.Features.DataAccess;

namespace Me.Hypes.Helpers;

public static class HypesHelper
{
    public static async Task<HashSet<long>> GetHypedGameIdsAsync(LudusContext db, Guid userId)
    {
        return (
            await db.Hypes.Where(w => w.UserId == userId).Select(w => w.GameId).ToListAsync()
        ).ToHashSet();
    }

    public static async Task<HashSet<long>> GetHypedGamesByIdsAsync(
        LudusContext db,
        Guid userId,
        HashSet<long> filterByGameIds
    )
    {
        return (
            await db
                .Hypes.Where(w => w.UserId == userId && filterByGameIds.Contains(w.GameId))
                .Select(w => w.GameId)
                .ToListAsync()
        ).ToHashSet();
    }

    public static async Task<bool> IsHypedAsync(LudusContext db, Guid userId, long gameId)
    {
        return await db.Hypes.AnyAsync(w => w.UserId == userId && w.GameId == gameId);
    }
}
