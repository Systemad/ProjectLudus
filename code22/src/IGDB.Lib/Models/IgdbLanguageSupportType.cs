using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbLanguageSupportType
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
