using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public partial class PlatformWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("trusted")]
    public bool Trusted { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}