namespace Shared;

public partial class PlatformFamilies
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}