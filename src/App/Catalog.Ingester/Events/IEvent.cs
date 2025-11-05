namespace Catalog.Ingester.Events;

public abstract class WebhookEvent
{
    public long Id { get; set; }
    public long CreatedAt { get; set; }
}