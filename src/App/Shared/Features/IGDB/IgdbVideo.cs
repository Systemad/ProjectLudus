using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public class IgdbVideo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("video_id")]
    public string VideoId { get; set; }
}
