using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public partial class IgdbRatingCategory
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("rating")]
    public string Rating { get; set; }
}
