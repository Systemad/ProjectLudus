using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public partial class IgdbGameWebsiteType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
