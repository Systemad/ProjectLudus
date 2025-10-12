using System.Text.Json.Serialization;
using Shared.Features.Games;

namespace Shared.Features.IGDB;

public class IgdbLanguageSupport
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("language")]
    public IgdbLanguage IgdbLanguage { get; set; }

    [JsonPropertyName("language_support_type")]
    public IgdbLanguageSupportType IgdbLanguageSupportType { get; set; }
}
