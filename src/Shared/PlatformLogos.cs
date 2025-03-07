namespace Shared;

public partial class PlatformLogos
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("alpha_channel")]
    public bool AlphaChannel { get; set; }

    [JsonPropertyName("animated")]
    public bool Animated { get; set; }

    [JsonPropertyName("height")]
    public long Height { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("width")]
    public long Width { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}