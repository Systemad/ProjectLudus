using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyncService.Data.Features.Companies;

public class CompanyEntityConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}  

public class RawCompanyEntityConfiguration : IEntityTypeConfiguration<RawCompanyEntity>
{
    public void Configure(EntityTypeBuilder<RawCompanyEntity> builder)
    {
        builder
            .OwnsOne(c => c.Payload, d =>
            {
                d.ToJson();
            })
            .Property(g => g.Id)
            .ValueGeneratedNever();

    }
}   