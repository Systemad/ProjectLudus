using CatalogAPI.Data.Entities;
using NodaTime;
using Shared.Features.IGDB;

namespace CatalogAPI.Data.Features.Companies;

public class CompanyEntity
{
    public long Id { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant Added { get; set; }
    
    public IgdbCompany Metadata { get; set; } = null!;

    
    public CompanyEntity? Parent { get; set; }
    public long? ParentId { get; set; }
    
    public long? ChangedCompanyId { get; set; }
    public CompanyEntity? ChangedCompany { get; set; }

    public virtual List<GameEntity> Games { get; set; } = [];
}
