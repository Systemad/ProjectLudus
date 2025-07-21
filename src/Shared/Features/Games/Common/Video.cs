using System.Text.Json.Serialization;

namespace Shared.Features.Games.Common;

public partial class Video
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("video_id")]
    public string VideoId { get; set; }
}
