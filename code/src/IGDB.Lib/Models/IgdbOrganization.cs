using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public partial class IgdbOrganization
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
