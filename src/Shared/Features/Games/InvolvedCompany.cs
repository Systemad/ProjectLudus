using System.Text.Json.Serialization;
using Shared.Features.Games;

namespace Shared.Features.Games;

public partial class InvolvedCompany
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("company")]
    public Company Company { get; set; }

    [JsonPropertyName("developer")]
    public bool Developer { get; set; }

    [JsonPropertyName("porting")]
    public bool Porting { get; set; }

    [JsonPropertyName("publisher")]
    public bool Publisher { get; set; }

    [JsonPropertyName("supporting")]
    public bool Supporting { get; set; }
}
