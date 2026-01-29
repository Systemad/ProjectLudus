using System.Text.Json.Serialization;
using IGDB;
using IGDB.Models;

namespace Catalog.Ingester.Webhooks;

public class WebhookDeleteGamePayload
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}
public class WebhookPayload
{
    [JsonPropertyName("payload")]
    public IdentityOrValue<Game> Payload { get; set; }
}