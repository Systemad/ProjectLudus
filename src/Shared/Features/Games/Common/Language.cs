using System.Text.Json.Serialization;

namespace Shared.Features.Games.Common;

public partial class Language
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("locale")]
    public string Locale { get; set; }
}
