using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public partial class IgdbReleaseRegion
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }
}
