using NodaTime;
using Shared.Features.IGDB;
using SyncService.Data.Features.Companies;

namespace SyncService.Features.Companies;

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