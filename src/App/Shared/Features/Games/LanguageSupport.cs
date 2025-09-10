using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class LanguageSupport
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("language")]
    public Language Language { get; set; }

    [JsonPropertyName("language_support_type")]
    public LanguageSupportType LanguageSupportType { get; set; }
}
