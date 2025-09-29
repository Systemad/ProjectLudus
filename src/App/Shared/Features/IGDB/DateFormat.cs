using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class DateFormat
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }
    
    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }
}