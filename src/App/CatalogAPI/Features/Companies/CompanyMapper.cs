using CatalogAPI.Data.Features.Companies;
using NodaTime;

namespace CatalogAPI.Features.Companies;

public static class CompanyMapper
{
    public static CompanyEntity ToEntity(this CompanyEntity igdbCompanyEntity)
    {
        return new CompanyEntity
        {
            Id = igdbCompanyEntity.Id,
            UpdatedAt = Instant.FromDateTimeUtc(DateTime.UtcNow),
            Added = Instant.FromDateTimeUtc(DateTime.UtcNow),
            Metadata = igdbCompanyEntity,
        };
    }
}
