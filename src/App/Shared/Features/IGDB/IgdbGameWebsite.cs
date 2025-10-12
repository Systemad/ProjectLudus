using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public class IgdbGameWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public IgdbGameWebsiteType Type { get; set; }
}
