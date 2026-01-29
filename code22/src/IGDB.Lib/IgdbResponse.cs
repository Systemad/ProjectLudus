using System.Text.Json.Serialization;

namespace IGDB.Lib;

public abstract class IgdbResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }
    
    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}