using CatalogAPI.Data.Features.Themes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.GameModes;

public class GameModeConfiguration : IEntityTypeConfiguration<ThemeEntity>
{
    public void Configure(EntityTypeBuilder<ThemeEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();

        //builder.HasData(GameModeData.All);
        

    }
}