using System.Text.Json.Serialization;

namespace Ludus.Seeder;

public class CountResponse
{
    [JsonPropertyName("count")]
    public long Count { get; set; }
}
