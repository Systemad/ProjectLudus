using System.Text.Json.Serialization;

namespace Shared.Features.Games.Common;

public partial class Company
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("country")]
    public long Country { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("websites")]
    public List<CompanyWebsite> Websites { get; set; }

    [JsonPropertyName("status")]
    public CompanyStatus Status { get; set; }
}
