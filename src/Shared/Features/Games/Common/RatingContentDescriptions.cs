using System.Text.Json.Serialization;

namespace Shared.Features.Games.Common;

public partial class RatingContentDescriptions
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
