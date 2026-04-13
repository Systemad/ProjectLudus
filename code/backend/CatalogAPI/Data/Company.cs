namespace CatalogAPI.Data;

public partial class Company
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public long? ChangeDate { get; set; }

    public long? ChangeDateCategory { get; set; }

    public long? Country { get; set; }

    public string? Description { get; set; }

    public long? Logo { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public long? StartDate { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public long? Status { get; set; }

    public long? StartDateFormat { get; set; }

    public long? ParentId { get; set; }

    public long? ChangedCompanyId { get; set; }

    public virtual Company? ChangedCompany { get; set; }

    public virtual ICollection<Company> InverseChangedCompany { get; set; } = new List<Company>();

    public virtual ICollection<Company> InverseParent { get; set; } = new List<Company>();

    public virtual ICollection<InvolvedCompany> InvolvedCompanies { get; set; } = new List<InvolvedCompany>();

    public virtual CompanyLogo? LogoNavigation { get; set; }

    public virtual Company? Parent { get; set; }

    public virtual ICollection<PlatformVersionCompany> PlatformVersionCompanies { get; set; } = new List<PlatformVersionCompany>();

    public virtual DateFormat? StartDateFormatNavigation { get; set; }

    public virtual CompanyStatus? StatusNavigation { get; set; }

    public virtual ICollection<GameEngine> GameEngines { get; set; } = new List<GameEngine>();

    public virtual ICollection<PlatformVersion> PlatformVersions { get; set; } = new List<PlatformVersion>();
}
