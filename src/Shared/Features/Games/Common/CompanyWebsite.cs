using System.Text.Json.Serialization;

namespace Shared.Features.Games.Common;

public partial class CompanyWebsite
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public long Type { get; set; }
}
