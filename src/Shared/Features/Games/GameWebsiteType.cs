using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class GameWebsiteType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
