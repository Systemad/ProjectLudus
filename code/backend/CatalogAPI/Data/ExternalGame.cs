namespace CatalogAPI.Data;

public partial class ExternalGame
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? Game { get; set; }

    public string? Name { get; set; }

    public string? Uid { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public long? Year { get; set; }

    public long? Category { get; set; }

    public long? Media { get; set; }

    public long? Platform { get; set; }

    public string? Countries { get; set; }

    public long? GameReleaseFormat { get; set; }

    public long? ExternalGameSource { get; set; }

    public virtual ExternalGameSource? ExternalGameSourceNavigation { get; set; }

    public virtual Game? GameNavigation { get; set; }

    public virtual GameReleaseFormat? GameReleaseFormatNavigation { get; set; }

    public virtual Platform? PlatformNavigation { get; set; }
}
