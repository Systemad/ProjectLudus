using System.Text.Json.Serialization;

namespace IGDBService.Responses;

public class CountResponse
{
    [JsonPropertyName("count")]
    public long Count { get; set; }
}
