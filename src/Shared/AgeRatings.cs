namespace Shared;

public partial class AgeRatings
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("content_descriptions")]
    public List<long> ContentDescriptions { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }

    [JsonPropertyName("organization")]
    public long Organization { get; set; }

    [JsonPropertyName("rating_category")]
    public long RatingCategory { get; set; }

    [JsonPropertyName("rating_content_descriptions")]
    public List<long> RatingContentDescriptions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("synopsis")]
    public string Synopsis { get; set; }
}