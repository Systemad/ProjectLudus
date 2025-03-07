namespace Shared;

public partial class PlatformWebsites
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    // TODO: Enum?
    [JsonPropertyName("category")]
    public long Category { get; set; }

    [JsonPropertyName("trusted")]
    public bool Trusted { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}

public enum WebsiteCategory
{
    official = 1,
    wikia = 2,
    wikipedia = 3,
    facebook = 4,
    twitter = 5,
    twitch = 6,
    instagram = 8,
    youtube = 9,
    iphone = 10,
    ipad = 11,
    android = 12,
    steam = 13,
    reddit = 14,
    discord = 15,
    google_plus = 16,
    tumblr = 17,
    linkedin = 18,
    pinterest = 19,
    soundcloud = 20
}