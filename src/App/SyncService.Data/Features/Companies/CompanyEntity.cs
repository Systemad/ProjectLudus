using NodaTime;
using Shared.Features.IGDB;
using SyncService.Data.Entities;

namespace SyncService.Data.Features.Companies;

public class CompanyEntity
{
    public long Id { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant Added { get; set; }
    
    public Company Metadata { get; set; } = null!;

    
    public CompanyEntity? Parent { get; set; }
    public long? ParentId { get; set; }
    
    public long? ChangedCompanyId { get; set; }
    public CompanyEntity? ChangedCompany { get; set; }

    public virtual List<GameEntity> Games { get; set; } = [];
}
