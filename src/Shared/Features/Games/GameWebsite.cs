using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class GameWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public GameWebsiteType Type { get; set; }
}
