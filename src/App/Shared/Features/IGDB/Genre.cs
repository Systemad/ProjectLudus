using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public class Genre : IgdbResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("slug")]
    public required string Slug { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }
}
