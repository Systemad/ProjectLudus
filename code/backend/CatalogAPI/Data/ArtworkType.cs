namespace CatalogAPI.Data;

public partial class ArtworkType
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Slug { get; set; }

    public string? Name { get; set; }

    public string? Checksum { get; set; }
}
