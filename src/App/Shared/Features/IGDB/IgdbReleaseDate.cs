using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public class IgdbReleaseDate
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("date")]
    public long Date { get; set; }

    [JsonPropertyName("human")]
    public string Human { get; set; }

    [JsonPropertyName("platform")]
    public IgdbReleasePlatform IgdbReleasePlatforms { get; set; }

    [JsonPropertyName("y")]
    public long Y { get; set; }

    [JsonPropertyName("date_format")]
    public long DateFormat { get; set; }

    [JsonPropertyName("release_region")]
    public IgdbReleaseRegion IgdbReleaseRegion { get; set; }

    [JsonPropertyName("status")]
    public long? Status { get; set; }
}
