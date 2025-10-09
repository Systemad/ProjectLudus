using CatalogAPI.Data.Features.Companies;
using NodaTime;
using Shared.Features.IGDB;

namespace CatalogAPI.Features.Companies;

public static class CompanyMapper
{
    public static CompanyEntity ToEntity(this Company company)
    {
        return new CompanyEntity
        {
            Id = company.Id,
            UpdatedAt = Instant.FromDateTimeUtc(DateTime.UtcNow),
            Added = Instant.FromDateTimeUtc(DateTime.UtcNow),
            Metadata = company,
        };
    }
}
