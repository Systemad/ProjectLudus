using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class GameType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
