using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class GameEngine
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("logo")]
    public GameEngineLogo Logo { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
