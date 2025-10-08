using System.Text.Json.Serialization;
using Shared.Features.Games;

namespace Shared.Features.IGDB;
public class GameEngine : IgdbResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("platforms")]
    public List<long>? Platforms { get; set; }
    
    [JsonPropertyName("logo")]
    public GameEngineLogo? GameEngineLogo { get; set; }

    [JsonPropertyName("slug")]
    public required string Slug { get; set; }
    
    [JsonPropertyName("url")]
    public required string Url { get; set; }
}
