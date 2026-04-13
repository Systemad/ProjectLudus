namespace CatalogAPI.Data;

public partial class AlternativeName
{
    public long Id { get; set; }

    public string? Comment { get; set; }

    public long? Game { get; set; }

    public string? Name { get; set; }

    public string? Checksum { get; set; }

    public virtual Game? GameNavigation { get; set; }
}
