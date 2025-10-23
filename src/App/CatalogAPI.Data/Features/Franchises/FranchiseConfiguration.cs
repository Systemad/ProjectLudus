using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.Franchises;

public class FranchiseConfiguration : IEntityTypeConfiguration<FranchiseEntity>
{
    public void Configure(EntityTypeBuilder<FranchiseEntity> builder)
    {
        builder.Property(g => g.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Metadata).HasColumnType("jsonb");
        
        //builder.ComplexProperty<Franchise>(b => b.Metadata, d => d.ToJson());
    }
}
