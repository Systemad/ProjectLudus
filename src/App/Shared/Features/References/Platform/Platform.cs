using System.Text.Json.Serialization;

namespace Shared.Features.References.Platform;

public class Platform : IgdbResponse
{
    [JsonPropertyName("abbreviation")]
    public string Abbreviation { get; set; }

    [JsonPropertyName("alternative_name")]
    public string AlternativeName { get; set; }

    [JsonPropertyName("generation")]
    public long? Generation { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("platform_logo")]
    public PlatformLogo PlatformLogo { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("websites")]
    public List<PlatformWebsite> Websites { get; set; }
}


