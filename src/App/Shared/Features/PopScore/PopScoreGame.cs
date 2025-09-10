using System.Text.Json.Serialization;

namespace Shared.Features.PopScore;

public partial class PopScoreGame
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("game_id")]
    public long GameId { get; set; }

    [JsonPropertyName("popularity_type")]
    public long PopularityType { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }
}