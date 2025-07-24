using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class AgeRating
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("content_descriptions")]
    public List<ContentDescription> ContentDescriptions { get; set; }

    [JsonPropertyName("organization")]
    public Organization Organization { get; set; }

    [JsonPropertyName("rating_category")]
    public RatingCategory RatingCategory { get; set; }

    [JsonPropertyName("rating_content_descriptions")]
    public List<RatingContentDescriptions> RatingContentDescriptions { get; set; }
}
