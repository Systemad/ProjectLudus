using System.Text.Json.Serialization;

namespace IGDB.Lib;

public class CountResponse
{
    [JsonPropertyName("count")]
    public long Count { get; set; }
}
