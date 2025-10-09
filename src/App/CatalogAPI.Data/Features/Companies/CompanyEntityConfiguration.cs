using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.Companies;

public class CompanyEntityConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}  
