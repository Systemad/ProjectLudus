using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public partial class IgdbReleaseRegion
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }
}
