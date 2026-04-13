namespace CatalogAPI.Data;

public partial class PlatformType
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
