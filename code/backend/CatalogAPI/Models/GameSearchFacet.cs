using System.Text.Json.Serialization;

namespace CatalogAPI.Models;

public partial class AggregationBuckets
{
    [JsonPropertyName("buckets")] public required Bucket[] Buckets { get; set; }

    [JsonPropertyName("sum_other_doc_count")]
    public required long SumOtherDocCount { get; set; }
}

public partial class Bucket
{
    [JsonPropertyName("key")] public required string Key { get; set; }

    [JsonPropertyName("doc_count")] public required long DocCount { get; set; }
}

public class GameSearchFacet
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Storyline { get; set; }
    public long? FirstReleaseDate { get; set; }
    public string? GameType { get; set; }
    public string? GameStatus { get; set; }
    public string? CoverUrl { get; set; }
    public float? Score { get; set; }
    public List<string>? Themes { get; set; }
    public List<string>? Genres { get; set; }
    public List<string>? GameModes { get; set; }
    public List<string>? Platforms { get; set; }
    public List<string>? GameEngines { get; set; }
    public List<string>? PlayerPerspectives { get; set; }
    public List<string>? Publishers { get; set; }
    public List<string>? Developers { get; set; }
    public List<string>? MultiplayerModes { get; set; }
    public int? ReleaseYear { get; set; }
}

public class GameItem
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Storyline { get; set; }
    public long? FirstReleaseDate { get; set; }
    public string? GameType { get; set; }
    public string? GameStatus { get; set; }
    public string? CoverUrl { get; set; }
    public float? Score { get; set; }
    public List<string>? Themes { get; set; }
    public List<string>? Genres { get; set; }
    public List<string>? GameModes { get; set; }
    public List<string>? Platforms { get; set; }
    public List<string>? GameEngines { get; set; }
    public List<string>? PlayerPerspectives { get; set; }
    public List<string>? Publishers { get; set; }
    public List<string>? Developers { get; set; }
    public List<string>? MultiplayerModes { get; set; }
    public int? ReleaseYear { get; set; }
}