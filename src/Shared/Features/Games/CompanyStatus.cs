using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class CompanyStatus
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
