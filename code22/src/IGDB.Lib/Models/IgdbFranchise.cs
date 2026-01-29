using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbFranchise
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("games")]
    public List<long>? Games { get; set; }
}
