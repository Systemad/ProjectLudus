using System.Text.Json.Serialization;

namespace Ludus.Worker;

public class CountResponse
{
    [JsonPropertyName("count")]
    public long Count { get; set; }
}
