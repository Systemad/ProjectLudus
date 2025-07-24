using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class AlternativeName
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
