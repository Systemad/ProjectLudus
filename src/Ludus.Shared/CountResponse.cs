using System.Text.Json.Serialization;

namespace Ludus.Shared;

public class CountResponse
{
    [JsonPropertyName("count")]
    public long Count { get; set; }
}
