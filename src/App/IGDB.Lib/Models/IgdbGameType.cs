using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbGameType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
