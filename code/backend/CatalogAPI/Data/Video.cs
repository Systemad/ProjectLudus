namespace CatalogAPI.Data;

public partial class Video
{
    public long Id { get; set; }

    public long? Game { get; set; }

    public string? Name { get; set; }

    public string? VideoId { get; set; }

    public string? Checksum { get; set; }

    public virtual Game? GameNavigation { get; set; }
}
