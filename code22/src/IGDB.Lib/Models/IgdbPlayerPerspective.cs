using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbPlayerPerspective : IgdbResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
