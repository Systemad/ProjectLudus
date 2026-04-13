namespace CatalogAPI.Data;

public partial class Website
{
    public long Id { get; set; }

    public long? Game { get; set; }

    public bool? Trusted { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public long? Type { get; set; }

    public virtual Game? GameNavigation { get; set; }

    public virtual WebsiteType? TypeNavigation { get; set; }
}
