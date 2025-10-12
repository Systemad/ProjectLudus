using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public class IgdbGameType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
