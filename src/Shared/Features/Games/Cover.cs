using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class Cover
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("animated")]
    public bool Animated { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
