using System.Text.Json.Serialization;

public record EndpointConfig
{
    public List<string> Ref { get; init; } = [];
    public List<string> Default { get; init; } = [];
}

public class IgdbBase
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public sealed class WebhookEvent
{
    public Guid Id { get; set; }
    public long EntityId { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }
    public string Endpoint { get; set; } = null!;
    public string EventType { get; set; } = null!;
    public string Payload { get; set; } = "";
    public bool Processed { get; set; }
}

public class WebhookResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("category")]
    public long Category { get; set; }

    [JsonPropertyName("sub_category")]
    public long SubCategory { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("api_key")]
    public required string ApiKey { get; set; }

    [JsonPropertyName("secret")]
    public required string Secret { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }
}

public partial class EntityDeleted
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public class IGDBOptions
{
    public const string IGDB = "IGDB";

    public string CLIENT_ID { get; set; } = "";
    public string ACCESS_TOKEN { get; set; } = "";
    public string WEBHOOK_URL { get; set; } = "";
    public string WEBHOOK_SECRET { get; set; } = "";
}
