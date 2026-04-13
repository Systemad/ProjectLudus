namespace CatalogAPI.Data;

public partial class PlatformVersion
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? PlatformLogo { get; set; }

    public string? Slug { get; set; }

    public string? Summary { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public string? Cpu { get; set; }

    public string? Media { get; set; }

    public string? Memory { get; set; }

    public string? Output { get; set; }

    public string? Resolutions { get; set; }

    public string? Sound { get; set; }

    public string? Connectivity { get; set; }

    public string? Storage { get; set; }

    public string? Graphics { get; set; }

    public string? Os { get; set; }

    public long? MainManufacturer { get; set; }

    public virtual PlatformVersionCompany? MainManufacturerNavigation { get; set; }

    public virtual PlatformLogo? PlatformLogoNavigation { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
