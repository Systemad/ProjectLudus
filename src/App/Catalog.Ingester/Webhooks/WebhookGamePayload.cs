using System.Text.Json.Serialization;

namespace Catalog.Ingester.Webhooks;

public class WebhookDeleteGamePayload
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}
public partial class WebhookGamePayload
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("age_ratings")] public List<long> AgeRatings { get; set; }

    [JsonPropertyName("aggregated_rating")]
    public double AggregatedRating { get; set; }

    [JsonPropertyName("aggregated_rating_count")]
    public long AggregatedRatingCount { get; set; }

    [JsonPropertyName("alternative_names")]
    public List<long> AlternativeNames { get; set; }

    [JsonPropertyName("artworks")] public List<long> Artworks { get; set; }

    [JsonPropertyName("cover")] public long Cover { get; set; }

    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }

    [JsonPropertyName("external_games")] public List<long> ExternalGames { get; set; }

    [JsonPropertyName("first_release_date")]
    public long FirstReleaseDate { get; set; }

    [JsonPropertyName("game_engines")] public List<long> GameEngines { get; set; }

    [JsonPropertyName("game_modes")] public List<long> GameModes { get; set; }

    [JsonPropertyName("genres")] public List<long> Genres { get; set; }

    [JsonPropertyName("involved_companies")]
    public List<long> InvolvedCompanies { get; set; }

    [JsonPropertyName("keywords")] public List<long> Keywords { get; set; }

    [JsonPropertyName("multiplayer_modes")]
    public List<long> MultiplayerModes { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("platforms")] public List<long> Platforms { get; set; }

    [JsonPropertyName("player_perspectives")]
    public List<long> PlayerPerspectives { get; set; }

    [JsonPropertyName("rating")] public double Rating { get; set; }

    [JsonPropertyName("rating_count")] public long RatingCount { get; set; }

    [JsonPropertyName("release_dates")] public List<long> ReleaseDates { get; set; }

    [JsonPropertyName("screenshots")] public List<long> Screenshots { get; set; }

    [JsonPropertyName("similar_games")] public List<long> SimilarGames { get; set; }

    [JsonPropertyName("slug")] public string Slug { get; set; }

    [JsonPropertyName("summary")] public string Summary { get; set; }

    [JsonPropertyName("tags")] public List<long> Tags { get; set; }

    [JsonPropertyName("themes")] public List<long> Themes { get; set; }

    [JsonPropertyName("total_rating")] public double TotalRating { get; set; }

    [JsonPropertyName("total_rating_count")]
    public long TotalRatingCount { get; set; }

    [JsonPropertyName("updated_at")] public long UpdatedAt { get; set; }

    [JsonPropertyName("url")] public Uri Url { get; set; }

    [JsonPropertyName("videos")] public List<long> Videos { get; set; }

    [JsonPropertyName("websites")] public List<long> Websites { get; set; }

    [JsonPropertyName("checksum")] public Guid Checksum { get; set; }

    [JsonPropertyName("language_supports")]
    public List<long> LanguageSupports { get; set; }

    [JsonPropertyName("collections")] public List<long> Collections { get; set; }

    [JsonPropertyName("game_type")] public long GameType { get; set; }
}