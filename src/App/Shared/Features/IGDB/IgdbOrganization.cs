using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public partial class IgdbOrganization
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
