using System.Text.Json.Serialization;
using Shared.Features.Games;

namespace Shared.Features;

public class IGDBGame
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("age_ratings")]
    public List<AgeRating> AgeRatings { get; set; } = new();

    [JsonPropertyName("alternative_names")]
    public List<AlternativeName> AlternativeNames { get; set; } = new();

    [JsonPropertyName("artworks")]
    public List<Artwork> Artworks { get; set; } = new();

    [JsonPropertyName("cover")]
    public Cover? Cover { get; set; }

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }

    [JsonPropertyName("dlcs")]
    public List<long> Dlcs { get; set; } = new();

    [JsonPropertyName("expansions")]
    public List<long> Expansions { get; set; } = new();

    [JsonPropertyName("first_release_date")]
    public long FirstReleaseDate { get; set; }

    [JsonPropertyName("franchises")]
    public List<Franchise> Franchises { get; set; } = new();

    [JsonPropertyName("game_engines")]
    public List<GameEngine> GameEngines { get; set; } = new();

    [JsonPropertyName("game_modes")]
    public List<GameMode> GameModes { get; set; } = new();

    [JsonPropertyName("genres")]
    public List<Genre> Genres { get; set; } = new();
    
    [JsonPropertyName("hypes")]
    public long? Hypes { get; set; }

    [JsonPropertyName("involved_companies")]
    public List<InvolvedCompanyRaw> InvolvedCompanies { get; set; } = new();

    [JsonPropertyName("keywords")]
    public List<Keyword> Keywords { get; set; } = new();

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("platforms")]
    public List<Platform> Platforms { get; set; } = new();

    [JsonPropertyName("player_perspectives")]
    public List<PlayerPerspective> PlayerPerspectives { get; set; } = new();

    [JsonPropertyName("rating")]
    public double? Rating { get; set; }

    [JsonPropertyName("rating_count")]
    public long? RatingCount { get; set; }

    [JsonPropertyName("release_dates")]
    public List<ReleaseDate> ReleaseDates { get; set; } = new();

    [JsonPropertyName("screenshots")]
    public List<Screenshot> Screenshots { get; set; } = new();

    [JsonPropertyName("similar_games")]
    public List<long> SimilarGames { get; set; } = new();

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("storyline")]
    public string? Storyline { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("themes")]
    public List<Theme> Themes { get; set; } = new();

    [JsonPropertyName("total_rating")]
    public double? TotalRating { get; set; }

    [JsonPropertyName("total_rating_count")]
    public long? TotalRatingCount { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("videos")]
    public List<Video> Videos { get; set; } = new();

    [JsonPropertyName("websites")]
    public List<GameWebsite> Websites { get; set; } = new();

    [JsonPropertyName("checksum")]
    public required string Checksum { get; set; }

    [JsonPropertyName("language_supports")]
    public List<LanguageSupport> LanguageSupports { get; set; } = new();

    [JsonPropertyName("collections")]
    public List<Collection> Collections { get; set; } = new();

    [JsonPropertyName("game_type")] public GameType GameType { get; set; } = new();

    [JsonPropertyName("multiplayer_modes")]
    public List<MultiplayerMode> MultiplayerModes { get; set; } = new();
}
