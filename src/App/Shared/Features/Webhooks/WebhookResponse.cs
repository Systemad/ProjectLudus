using System.Text.Json.Serialization;
using NodaTime;

namespace Shared.Features.Webhooks;

public class ActiveWebhook
{
    public long Id { get; set; }
    public string Url { get; set; }
    public long Category { get; set; }
    public long SubCategory { get; set; }
    public bool Active { get; set; }
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
    public Instant LastProcessedAt { get; set; }
}

public partial class WebhookResponse
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("category")] public long Category { get; set; }

    [JsonPropertyName("sub_category")] public long SubCategory { get; set; }

    [JsonPropertyName("active")] public bool Active { get; set; }

    [JsonPropertyName("api_key")] public string ApiKey { get; set; }

    [JsonPropertyName("secret")] public string Secret { get; set; }

    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }

    [JsonPropertyName("updated_at")] public long UpdatedAt { get; set; }
}