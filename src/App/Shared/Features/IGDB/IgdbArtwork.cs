using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class IgdbArtwork
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("animated")]
    public bool? Animated { get; set; }
}
