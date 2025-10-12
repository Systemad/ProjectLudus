using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public class IgdbPlayerPerspective : IgdbResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
