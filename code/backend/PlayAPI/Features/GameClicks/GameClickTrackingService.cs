using Microsoft.EntityFrameworkCore;
using PlayAPI.Context;
using PlayAPI.Data;

namespace PlayAPI.Features.GameClicks;

public class GameClickTrackingService
{
    private readonly AppDbContext _db;

    public GameClickTrackingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<GameClickRecord> RecordClickAsync(
        long gameId,
        CancellationToken cancellationToken = default
    )
    {
        var visitedAt = DateTime.UtcNow;

        var visitCount = await _db
            .GameVisitCounts.AsTracking()
            .SingleOrDefaultAsync(x => x.GameId == gameId, cancellationToken);

        if (visitCount == null)
        {
            visitCount = new GameVisitCount
            {
                GameId = gameId,
                Count = 1,
                LastVisitedAt = visitedAt,
            };

            _db.GameVisitCounts.Add(visitCount);
        }
        else
        {
            visitCount.Count += 1;
            visitCount.LastVisitedAt = visitedAt;
        }

        await _db.SaveChangesAsync(cancellationToken);

        return new GameClickRecord(visitCount.GameId, visitCount.Count, visitCount.LastVisitedAt);
    }

    public sealed record GameClickRecord(long GameId, long Count, DateTime LastVisitedAt);
}
