using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class RatingCategory
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("rating")]
    public string Rating { get; set; }
}
