using Catalog.Themes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.Themes;

public class ThemeConfiguration : IEntityTypeConfiguration<ThemeEntity>
{
    public void Configure(EntityTypeBuilder<ThemeEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}