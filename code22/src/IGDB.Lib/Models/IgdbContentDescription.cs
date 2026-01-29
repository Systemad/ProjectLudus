using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public partial class IgdbContentDescription
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
