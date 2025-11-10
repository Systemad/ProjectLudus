using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public partial class IgdbGameWebsiteType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
