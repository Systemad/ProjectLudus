using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class ContentDescription
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
