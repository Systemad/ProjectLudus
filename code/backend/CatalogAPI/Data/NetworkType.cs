namespace CatalogAPI.Data;

public partial class NetworkType
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? EventNetworks { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<EventNetwork> EventNetworksNavigation { get; set; } = new List<EventNetwork>();
}
