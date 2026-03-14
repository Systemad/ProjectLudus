using System.Text.Json.Serialization;

namespace CatalogAPI.Models;

public partial class AggregationBuckets
{
    [JsonPropertyName("buckets")]
    public Bucket[] Buckets { get; set; }

    [JsonPropertyName("sum_other_doc_count")]
    public long SumOtherDocCount { get; set; }

    [JsonPropertyName("doc_count_error_upper_bound")]
    public long DocCountErrorUpperBound { get; set; }
}

public partial class Bucket
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("doc_count")]
    public long DocCount { get; set; }
}

public class GameSearchFacet
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Storyline { get; set; }
    public long? FirstReleaseDate { get; set; }
    public long? GameType { get; set; }
    public string? CoverUrl { get; set; }
    public int? ReleaseYear { get; set; }
    public float Score { get; set; }
    public List<string>? Themes { get; set; }
    public List<string>? Genres { get; set; }
    public List<string>? Modes { get; set; }
}


public sealed class AggregateResultCount
{
    [JsonPropertyName("value")]
    public long Value { get; init; }
}

public class GameItem
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Storyline { get; set; }
    public long? FirstReleaseDate { get; set; }
    public long? GameType { get; set; }
    public string? CoverUrl { get; set; }
    public int? ReleaseYear { get; set; }
    public long? TotalItems { get; set; }
    public float? Score { get; set; }
    public List<string>? Themes { get; set; }
    public List<string>? Genres { get; set; }
    public List<string>? Modes { get; set; }
    public AggregationBuckets? GenreFacet { get; set; } 
    public AggregationBuckets? ThemeFacet { get; set; }
}