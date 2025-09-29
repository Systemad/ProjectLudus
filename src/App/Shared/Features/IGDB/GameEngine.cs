using System.Text.Json.Serialization;

namespace Shared.Features.Games;

// TODO: FETCH SEPERATELY   
public class GameEngine
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

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
    [JsonPropertyName("updated_at")]
    public required long UpdatedAt { get; set; }
}
