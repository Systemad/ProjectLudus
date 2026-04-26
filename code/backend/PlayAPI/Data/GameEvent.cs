using NodaTime;

namespace PlayAPI.Data;

public class GameEvent
{
    public long Id { get; private set; }
    public long GameId { get; init; }
    public GameEventType EventType { get; init; }
    public Guid SessionId { get; init; }
    public Instant CreatedAt { get; init; }
}
