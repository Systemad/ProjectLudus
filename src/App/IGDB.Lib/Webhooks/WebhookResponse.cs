using System.Text.Json.Serialization;

namespace IGDB.Lib.Webhooks;

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