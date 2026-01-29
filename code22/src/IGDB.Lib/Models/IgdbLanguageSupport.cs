using System.Text.Json.Serialization;

namespace IGDB.Lib.Models;

public class IgdbLanguageSupport
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("language")]
    public IgdbLanguage IgdbLanguage { get; set; }

    [JsonPropertyName("language_support_type")]
    public IgdbLanguageSupportType IgdbLanguageSupportType { get; set; }
}
