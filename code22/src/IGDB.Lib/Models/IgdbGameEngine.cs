using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;
public class IgdbGameEngine : IgdbResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("platforms")]
    public List<long>? Platforms { get; set; }
    
    [JsonPropertyName("logo")]
    public IgdbGameEngineLogo? GameEngineLogo { get; set; }

    [JsonPropertyName("slug")]
    public required string Slug { get; set; }
    
    [JsonPropertyName("url")]
    public required string Url { get; set; }
}
