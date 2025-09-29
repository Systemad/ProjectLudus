using System.Text.Json.Serialization;
using Shared.Features.Games;

namespace Shared.Features.IGDB;

public class AgeRating
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
