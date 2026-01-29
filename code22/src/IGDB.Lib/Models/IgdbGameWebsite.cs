using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbGameWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public IgdbGameWebsiteType Type { get; set; }
}
