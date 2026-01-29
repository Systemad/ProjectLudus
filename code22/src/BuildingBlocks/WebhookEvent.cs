using BuildingBlocks.Twitch;

namespace BuildingBlocks;

public enum EventType
{
    Created,
    Updated,
    Deleted,
}

public class WebhookEvent
{
    public long ResourceId { get; set; }
    public long CreatedAt { get; set; }
    public IgdbType ResourceType { get; set; }
    public EventType EventType { get; set; }
    public string PayloadJson { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
