using Catalog.Games.Dtos;
using IGDB.Models;
using NodaTime;

namespace Catalog.Companies.Dtos;

public class CompanyDto
{
    public long Id { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant Added { get; set; }
    
    public Company Metadata { get; set; } = null!;
    
    public CompanyDto? Parent { get; set; }
    public CompanyDto? ChangedCompany { get; set; }
    public List<GameDto>? Games { get; set; } = [];
}