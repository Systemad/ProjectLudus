namespace Shared;

public partial class DateFormats
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}