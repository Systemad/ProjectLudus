using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class GameEngineLogo
{
    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("image_id")]
    public required string ImageId { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }
}
