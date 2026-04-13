namespace CatalogAPI.Data;

public partial class CompanyStatus
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
