namespace Shared;

public partial class Game
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("age_ratings")]
    public List<AgeRating> AgeRatings { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("artworks")]
    public List<Cover> Artworks { get; set; }

    [JsonPropertyName("cover")] public Cover Cover { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("first_release_date")]
    public long? FirstReleaseDate { get; set; }

    [JsonPropertyName("game_modes")] public List<GameEngine> GameModes { get; set; }

    [JsonPropertyName("genres")] public List<GameEngine> Genres { get; set; }

    [JsonPropertyName("involved_companies")]
    public List<InvolvedCompany> InvolvedCompanies { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("platforms")] public List<GameEngine> Platforms { get; set; }

    [JsonPropertyName("release_dates")] public List<ReleaseDate> ReleaseDates { get; set; }

    [JsonPropertyName("screenshots")] public List<Cover> Screenshots { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("similar_games")]
    public List<SimilarGame> SimilarGames { get; set; }

    [JsonPropertyName("slug")] public string Slug { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("themes")]
    public List<GameEngine> Themes { get; set; }

    [JsonPropertyName("url")] public Uri Url { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("websites")]
    public List<Website> Websites { get; set; }

    [JsonPropertyName("game_type")] public long GameType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("game_engines")]
    public List<GameEngine> GameEngines { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("keywords")]
    public List<GameEngine> Keywords { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("player_perspectives")]
    public List<GameEngine> PlayerPerspectives { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("storyline")]
    public string Storyline { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("summary")]
    public string Summary { get; set; }
}

public partial class AgeRating
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("organization")] public long Organization { get; set; }

    [JsonPropertyName("rating_category")] public long RatingCategory { get; set; }
}

public partial class Cover
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("image_id")] public string ImageId { get; set; }
}

public partial class GameEngine
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
}

public partial class InvolvedCompany
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("company")] public GameEngine Company { get; set; }

    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }

    [JsonPropertyName("developer")] public bool Developer { get; set; }

    [JsonPropertyName("game")] public long Game { get; set; }

    [JsonPropertyName("porting")] public bool Porting { get; set; }

    [JsonPropertyName("publisher")] public bool Publisher { get; set; }

    [JsonPropertyName("supporting")] public bool Supporting { get; set; }

    [JsonPropertyName("updated_at")] public long UpdatedAt { get; set; }

    [JsonPropertyName("checksum")] public string Checksum { get; set; }
}

public partial class ReleaseDate
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("category")] public long Category { get; set; }

    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("date")]
    public long? Date { get; set; }

    [JsonPropertyName("game")] public long Game { get; set; }

    [JsonPropertyName("human")] public string Human { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("m")]
    public long? M { get; set; }

    [JsonPropertyName("platform")] public GameEngine Platform { get; set; }

    [JsonPropertyName("region")] public long Region { get; set; }

    [JsonPropertyName("updated_at")] public long UpdatedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("y")]
    public long? Y { get; set; }

    [JsonPropertyName("checksum")] public string Checksum { get; set; }

    [JsonPropertyName("date_format")] public long DateFormat { get; set; }

    [JsonPropertyName("release_region")] public long ReleaseRegion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("status")]
    public long? Status { get; set; }
}

public partial class SimilarGame
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("cover")] public Cover Cover { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
}

public partial class Website
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("category")] public long Category { get; set; }

    [JsonPropertyName("game")] public long Game { get; set; }

    [JsonPropertyName("trusted")] public bool Trusted { get; set; }

    [JsonPropertyName("url")] public Uri Url { get; set; }

    [JsonPropertyName("checksum")] public string Checksum { get; set; }

    [JsonPropertyName("type")] public long Type { get; set; }
}
