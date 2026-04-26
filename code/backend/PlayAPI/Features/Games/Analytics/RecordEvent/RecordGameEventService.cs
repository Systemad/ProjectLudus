using Microsoft.EntityFrameworkCore;
using NodaTime;
using PlayAPI.Context;
using PlayAPI.Data;

namespace PlayAPI.Features.Games.Analytics.RecordEvent;

public class RecordGameEventService
{
    private readonly AppDbContext _db;

    public RecordGameEventService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<GameEvent> RecordAsync(
        long gameId,
        GameEventType eventType,
        Guid sessionId,
        CancellationToken cancellationToken = default
    )
    {
        var visitedAt = SystemClock.Instance.GetCurrentInstant();

        var gameEvent = new GameEvent
        {
            GameId = gameId,
            EventType = eventType,
            SessionId = sessionId,
            CreatedAt = visitedAt,
        };

        _db.GameEvents.Add(gameEvent);

        if (eventType == GameEventType.View)
        {
            var gameMetric = await _db
                .GameMetrics.AsTracking()
                .FirstOrDefaultAsync(
                    s => s.SessionId == sessionId && s.GameId == gameId,
                    cancellationToken
                );

            if (gameMetric == null)
            {
                gameMetric = new GameMetric
                {
                    SessionId = sessionId,
                    GameId = gameId,
                    FirstVisitedAt = visitedAt,
                    LastVisitedAt = visitedAt,
                    ViewCount = 1,
                };

                _db.GameMetrics.Add(gameMetric);
            }
            else
            {
                gameMetric.LastVisitedAt = visitedAt;
                gameMetric.ViewCount += 1;
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        return gameEvent;
    }
}
