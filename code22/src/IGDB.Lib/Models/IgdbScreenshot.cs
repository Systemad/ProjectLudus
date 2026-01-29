using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbScreenshot
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
