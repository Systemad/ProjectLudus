using System.Text.Json.Serialization;

namespace Ludus.Shared;

public partial class Game
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("age_ratings")]
    public List<AgeRating> AgeRatings { get; set; }

    [JsonPropertyName("alternative_names")]
    public List<AlternativeName> AlternativeNames { get; set; }

    [JsonPropertyName("category")]
    public Category Category { get; set; }

    [JsonPropertyName("cover")]
    public Cover Cover { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("first_release_date")]
    public long FirstReleaseDate { get; set; }

    [JsonPropertyName("game_engines")]
    public List<GameEngine> GameEngines { get; set; }

    [JsonPropertyName("game_modes")]
    public List<GameMode> GameModes { get; set; }

    [JsonPropertyName("genres")]
    public List<GameMode> Genres { get; set; }

    [JsonPropertyName("hypes")]
    public long Hypes { get; set; }

    [JsonPropertyName("involved_companies")]
    public List<InvolvedCompany> InvolvedCompanies { get; set; }

    [JsonPropertyName("multiplayer_modes")]
    public List<long> MultiplayerModes { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("platforms")]
    public List<Platform> Platforms { get; set; }

    [JsonPropertyName("player_perspectives")]
    public List<GameMode> PlayerPerspectives { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("rating_count")]
    public long RatingCount { get; set; }

    [JsonPropertyName("release_dates")]
    public List<ReleaseDate> ReleaseDates { get; set; }

    [JsonPropertyName("screenshots")]
    public List<Cover> Screenshots { get; set; }

    [JsonPropertyName("similar_games")]
    public List<SimilarGame> SimilarGames { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("tags")]
    public List<long> Tags { get; set; }

    [JsonPropertyName("themes")]
    public List<Theme> Themes { get; set; }

    [JsonPropertyName("total_rating")]
    public double TotalRating { get; set; }

    [JsonPropertyName("total_rating_count")]
    public long TotalRatingCount { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("videos")]
    public List<Video> Videos { get; set; }

    [JsonPropertyName("websites")]
    public List<Website> Websites { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }

    [JsonPropertyName("language_supports")]
    public List<LanguageSupport> LanguageSupports { get; set; }

    [JsonPropertyName("game_localizations")]
    public List<GameLocalization> GameLocalizations { get; set; }

    [JsonPropertyName("collections")]
    public List<Collection> Collections { get; set; }

    [JsonPropertyName("game_type")]
    public GameType GameType { get; set; }
}

public partial class AgeRating
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("category")]
    public long Category { get; set; }

    [JsonPropertyName("rating")]
    public long Rating { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }

    [JsonPropertyName("organization")]
    public GameMode Organization { get; set; }

    [JsonPropertyName("rating_category")]
    public RatingCategory RatingCategory { get; set; }
}

public partial class GameMode
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public partial class RatingCategory
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("rating")]
    public string Rating { get; set; }
}

public partial class AlternativeName
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    [JsonPropertyName("game")]
    public long Game { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}

public partial class Collection
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }
}

public partial class Cover
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }
}

public partial class GameEngine
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("companies")]
    public List<long> Companies { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("platforms")]
    public List<long> Platforms { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}

public partial class GameLocalization
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("region")]
    public long Region { get; set; }
}

public partial class GameType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public partial class InvolvedCompany
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("company")]
    public Company Company { get; set; }

    [JsonPropertyName("developer")]
    public bool Developer { get; set; }

    [JsonPropertyName("publisher")]
    public bool Publisher { get; set; }
}

public partial class Company
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("logo")]
    public Logo Logo { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}

public partial class Logo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("alpha_channel")]
    public bool AlphaChannel { get; set; }

    [JsonPropertyName("animated")]
    public bool Animated { get; set; }

    [JsonPropertyName("height")]
    public long Height { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("width")]
    public long Width { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}

public partial class LanguageSupport
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("language")]
    public GameMode Language { get; set; }
}

public partial class Platform
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}

public partial class ReleaseDate
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("date")]
    public long Date { get; set; }

    [JsonPropertyName("platform")]
    public long Platform { get; set; }
}

public partial class SimilarGame
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("category")]
    public long Category { get; set; }

    [JsonPropertyName("cover")]
    public Cover Cover { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}

public partial class Theme
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}

public partial class Video
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("video_id")]
    public string VideoId { get; set; }
}

public partial class Website
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("category")]
    public long Category { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }
}

public enum Category
{
    MainGame = 0,
    DlcAddon = 1,
    Expansion = 2,
    Bundle = 3,
    StandaloneExpansion = 4,
    Mod = 5,
    Episode = 6,
    Season = 7,
    Remake = 8,
    Remaster = 9,
    ExpandedGame = 10,
    Port = 11,
    Fork = 12,
    Pack = 13,
    Update = 14
}

public enum AgeRatingCategory
{
    ESRB = 1,
    PEGI = 2,
    CERO = 3,
    USK = 4,
    GRAC = 5,
    CLASS_IND = 6,
    ACB = 7
}