namespace CatalogAPI.Data;

public partial class ReleaseDateStatus
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<ReleaseDate> ReleaseDates { get; set; } = new List<ReleaseDate>();
}
