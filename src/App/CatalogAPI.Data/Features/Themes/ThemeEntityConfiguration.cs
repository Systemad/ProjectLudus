using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.Themes;

public class ThemeEntityConfiguration : IEntityTypeConfiguration<ThemeEntity>
{
    public void Configure(EntityTypeBuilder<ThemeEntity> builder)
    {
        //builder.ToTable("themes");

        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(60);

    }
}