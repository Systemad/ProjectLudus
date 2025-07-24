using System.Text.Json.Serialization;

namespace Shared.Twitch;

public class CountResponse
{
    [JsonPropertyName("count")]
    public long Count { get; set; }
}
