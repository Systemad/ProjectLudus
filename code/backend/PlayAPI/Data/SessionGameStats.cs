using NodaTime;

namespace PlayAPI.Data;

public class GameMetric
{
    public int Id { get; set; }
    public Guid SessionId { get; set; }
    public long GameId { get; set; }
    public Instant FirstVisitedAt { get; set; }
    public Instant LastVisitedAt { get; set; }
    public int ViewCount { get; set; }
}
