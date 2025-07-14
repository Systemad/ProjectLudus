using Microsoft.EntityFrameworkCore;
using WebAPI.Features.DataAccess;

namespace Me.Wishlists.Helpers;

public static class WishlistsHelper
{
    public static async Task<HashSet<long>> GetWishlistedGameIdsAsync(LudusContext db, Guid userId)
    {
        return (
            await db.Wishlists.Where(w => w.UserId == userId).Select(w => w.GameId).ToListAsync()
        ).ToHashSet();
    }

    public static async Task<HashSet<long>> GetWishlistedGamesByIdsAsync(
        LudusContext db,
        Guid userId,
        HashSet<long> filterByGameIds
    )
    {
        return (
            await db
                .Wishlists.Where(w => w.UserId == userId && filterByGameIds.Contains(w.GameId))
                .Select(w => w.GameId)
                .ToListAsync()
        ).ToHashSet();
    }

    public static async Task<bool> IsWishlistedAsync(LudusContext db, Guid userId, long gameId)
    {
        return await db.Wishlists.AnyAsync(w => w.UserId == userId && w.GameId == gameId);
    }
}
