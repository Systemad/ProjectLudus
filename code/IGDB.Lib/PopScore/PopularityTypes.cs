using System.Text.Json.Serialization;

namespace IGDB.Lib.PopScore;

public partial class PopularityTypes
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("popularity_source")]
    public long PopularitySource { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("external_popularity_source")]
    public ExternalPopularitySource ExternalPopularitySource { get; set; }
}

public partial class ExternalPopularitySource
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}