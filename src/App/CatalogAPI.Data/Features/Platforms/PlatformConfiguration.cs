using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.Platforms;

public class PlatformConfiguration : IEntityTypeConfiguration<PlatformEntity>
{
    public void Configure(EntityTypeBuilder<PlatformEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
        
        builder.ComplexProperty<PlatformLogo>(b => b.Logo);
    }
}