namespace CatalogAPI.Data;

public partial class Region
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Category { get; set; }

    public string? Identifier { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<GameLocalization> GameLocalizations { get; set; } = new List<GameLocalization>();
}
