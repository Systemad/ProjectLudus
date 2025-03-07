namespace Shared;

public class CountResponse
{
    [JsonPropertyName("count")] public long Count { get; set; }
}