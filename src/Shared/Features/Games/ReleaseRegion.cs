using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class ReleaseRegion
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }
}
