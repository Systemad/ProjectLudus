using Catalog.Games.Models;
using IGDB.Models;
using NodaTime;

namespace Catalog.Companies.Models;

public class CompanyEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Url { get; set; }
    public Instant StartDate { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant CreatedAt { get; set; }
    public Company Metadata { get; set; } = null!;
    
    public long? ParentId { get; set; }
    public CompanyEntity? Parent { get; set; }
    
    public long? ChangedCompanyId { get; set; }
    public CompanyEntity? ChangedCompany { get; set; } 

    public List<GameEntity> Developed { get; set; } = [];
    public List<GameEntity> Published { get; set; } = [];
}
