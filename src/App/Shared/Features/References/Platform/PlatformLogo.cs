using System.Text.Json.Serialization;

namespace Shared.Features.References.Platform;

public class PlatformLogo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
