using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public partial class IgdbWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("trusted")]
    public bool Trusted { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public IgdbTypeClass IgdbType { get; set; }
    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }
}

public partial class IgdbTypeClass
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }
}