using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class RawGame
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("age_ratings")]
    public List<AgeRating> AgeRatings { get; set; }

    [JsonPropertyName("alternative_names")]
    public List<AlternativeName> AlternativeNames { get; set; }

    [JsonPropertyName("artworks")]
    public List<Artwork> Artworks { get; set; }

    [JsonPropertyName("cover")]
    public Cover Cover { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("dlcs")]
    public List<Dlc> Dlcs { get; set; }

    [JsonPropertyName("expansions")]
    public List<Expansion> Expansions { get; set; }

    [JsonPropertyName("first_release_date")]
    public long FirstReleaseDate { get; set; }

    [JsonPropertyName("franchises")]
    public List<Franchise> Franchises { get; set; }

    [JsonPropertyName("game_engines")]
    public List<GameEngine> GameEngines { get; set; }

    [JsonPropertyName("game_modes")]
    public List<GameMode> GameModes { get; set; }

    [JsonPropertyName("genres")]
    public List<Genre> Genres { get; set; }

    [JsonPropertyName("hypes")]
    public long? Hypes { get; set; }

    [JsonPropertyName("involved_companies")]
    public List<InvolvedCompany> InvolvedCompanies { get; set; }

    [JsonPropertyName("keywords")]
    public List<Keyword> Keywords { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("platforms")]
    public List<Platform> Platforms { get; set; }

    [JsonPropertyName("player_perspectives")]
    public List<PlayerPerspective> PlayerPerspectives { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("rating_count")]
    public long RatingCount { get; set; }

    [JsonPropertyName("release_dates")]
    public List<ReleaseDate> ReleaseDates { get; set; }

    [JsonPropertyName("screenshots")]
    public List<Screenshot> Screenshots { get; set; }

    [JsonPropertyName("similar_games")]
    public List<SimilarGame> SimilarGames { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("storyline")]
    public string Storyline { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("themes")]
    public List<Theme> Themes { get; set; }

    [JsonPropertyName("total_rating")]
    public double TotalRating { get; set; }

    [JsonPropertyName("total_rating_count")]
    public long TotalRatingCount { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("videos")]
    public List<Video> Videos { get; set; }

    [JsonPropertyName("websites")]
    public List<GameWebsite> Websites { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }

    [JsonPropertyName("language_supports")]
    public List<LanguageSupport> LanguageSupports { get; set; }

    [JsonPropertyName("collections")]
    public List<Collection> Collections { get; set; }

    [JsonPropertyName("game_type")]
    public GameType GameType { get; set; }

    [JsonPropertyName("multiplayer_modes")]
    public List<MultiplayerMode> MultiplayerModes { get; set; }
}

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

public partial class ContentDescription
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public partial class RatingContentDescriptions
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public partial class AlternativeName
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public partial class Organization
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

public partial class Screenshot
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("animated")]
    public bool? Animated { get; set; }
}

public partial class Artwork
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("animated")]
    public bool? Animated { get; set; }
}

public partial class Franchise
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class GameMode
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class Genre
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class Keyword
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class PlayerPerspective
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class Theme
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
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
    public string Url { get; set; }
}

public partial class Cover
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("animated")]
    public bool Animated { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class SimilarGame
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public partial class Dlc
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public partial class Expansion
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public partial class GameEngine
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("logo")]
    public Logo Logo { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class Logo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class GameType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public partial class InternalGameType
{
    public long Id { get; set; }
    public long OriginalId { get; set; }
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

    [JsonPropertyName("porting")]
    public bool Porting { get; set; }

    [JsonPropertyName("publisher")]
    public bool Publisher { get; set; }

    [JsonPropertyName("supporting")]
    public bool Supporting { get; set; }
}

public partial class Company
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("country")]
    public long Country { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("websites")]
    public List<CompanyWebsite> Websites { get; set; }

    [JsonPropertyName("status")]
    public CompanyStatus Status { get; set; }
}

public partial class CompanyStatus
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public partial class CompanyWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public long Type { get; set; }
}

public partial class LanguageSupport
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("language")]
    public Language Language { get; set; }

    [JsonPropertyName("language_support_type")]
    public AlternativeName LanguageSupportType { get; set; }
}

public partial class Language
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("locale")]
    public string Locale { get; set; }
}

public partial class MultiplayerMode
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("campaigncoop")]
    public bool Campaigncoop { get; set; }

    [JsonPropertyName("lancoop")]
    public bool Lancoop { get; set; }

    [JsonPropertyName("offlinecoop")]
    public bool Offlinecoop { get; set; }

    [JsonPropertyName("onlinecoop")]
    public bool Onlinecoop { get; set; }

    [JsonPropertyName("splitscreen")]
    public bool Splitscreen { get; set; }

    [JsonPropertyName("offlinecoopmax")]
    public long? Offlinecoopmax { get; set; }

    [JsonPropertyName("offlinemax")]
    public long? Offlinemax { get; set; }

    [JsonPropertyName("onlinecoopmax")]
    public long? Onlinecoopmax { get; set; }

    [JsonPropertyName("onlinemax")]
    public long? Onlinemax { get; set; }
}

public partial class Platform
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("abbreviation")]
    public string Abbreviation { get; set; }

    [JsonPropertyName("generation")]
    public long? Generation { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("platform_logo")]
    public Logo PlatformLogo { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}

public partial class ReleaseDate
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("date")]
    public long Date { get; set; }

    [JsonPropertyName("human")]
    public string Human { get; set; }

    [JsonPropertyName("platform")]
    public ReleasePlatform ReleasePlatforms { get; set; }

    [JsonPropertyName("y")]
    public long Y { get; set; }

    [JsonPropertyName("date_format")]
    public long DateFormat { get; set; }

    [JsonPropertyName("release_region")]
    public ReleaseRegion ReleaseRegion { get; set; }

    [JsonPropertyName("status")]
    public long? Status { get; set; }
}

public partial class ReleasePlatform
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public partial class ReleaseRegion
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }
}

public partial class Video
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("video_id")]
    public string VideoId { get; set; }
}

public partial class GameWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public GameWebsiteType Type { get; set; }
}

public partial class GameWebsiteType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
