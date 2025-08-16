using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class MultiplayerMode
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("campaigncoop")]
    public bool Campaigncoop { get; set; }

    [JsonPropertyName("lancoop")]
    public bool Lancoop { get; set; }

    [JsonPropertyName("offlinecoop")]
    public bool Offlinecoop { get; set; }

    [JsonPropertyName("onlinecoop")]
    public bool Onlinecoop { get; set; }

    [JsonPropertyName("splitscreen")]
    public bool Splitscreen { get; set; }

    [JsonPropertyName("offlinecoopmax")]
    public long? Offlinecoopmax { get; set; }

    [JsonPropertyName("offlinemax")]
    public long? Offlinemax { get; set; }

    [JsonPropertyName("onlinecoopmax")]
    public long? Onlinecoopmax { get; set; }

    [JsonPropertyName("onlinemax")]
    public long? Onlinemax { get; set; }
}
