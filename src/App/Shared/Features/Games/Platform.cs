using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class Platform : IIdentifiable
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("abbreviation")]
    public string? Abbreviation { get; set; }

    [JsonPropertyName("generation")]
    public long? Generation { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("platform_logo")]
    public PlatformLogo? PlatformLogo { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
}
