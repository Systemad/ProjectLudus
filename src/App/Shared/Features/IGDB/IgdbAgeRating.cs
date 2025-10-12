using System.Text.Json.Serialization;
using Shared.Features.Games;

namespace Shared.Features.IGDB;

public class IgdbAgeRating
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("content_descriptions")]
    public List<IgdbContentDescription> ContentDescriptions { get; set; }

    [JsonPropertyName("organization")]
    public IgdbOrganization IgdbOrganization { get; set; }

    [JsonPropertyName("rating_category")]
    public IgdbRatingCategory IgdbRatingCategory { get; set; }

    [JsonPropertyName("rating_content_descriptions")]
    public List<IgdbRatingContentDescriptions> RatingContentDescriptions { get; set; }
}
