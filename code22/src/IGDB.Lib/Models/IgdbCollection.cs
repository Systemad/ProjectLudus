using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbCollection
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
