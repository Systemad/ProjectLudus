using System.Text.Json.Serialization;

namespace Shared.Features.IGDB;

public class InvolvedCompany
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("company")]
    public long CompanyId { get; set; }

    [JsonPropertyName("developer")]
    public bool Developer { get; set; }

    [JsonPropertyName("porting")]
    public bool Porting { get; set; }

    [JsonPropertyName("publisher")]
    public bool Publisher { get; set; }

    [JsonPropertyName("supporting")]
    public bool Supporting { get; set; }
}
