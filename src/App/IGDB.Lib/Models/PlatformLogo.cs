using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class PlatformLogo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
