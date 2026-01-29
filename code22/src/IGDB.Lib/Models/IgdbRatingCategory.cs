using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public partial class IgdbRatingCategory
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("rating")]
    public string Rating { get; set; }
}
