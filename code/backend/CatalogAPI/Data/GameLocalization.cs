namespace CatalogAPI.Data;

public partial class GameLocalization
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? Game { get; set; }

    public long? Region { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Checksum { get; set; }

    public long? Cover { get; set; }

    public virtual Cover? CoverNavigation { get; set; }

    public virtual Game? GameNavigation { get; set; }

    public virtual Region? RegionNavigation { get; set; }
}
