using Catalog.GameModes;
using Catalog.GameModes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.GameModes;

public class GameModeConfiguration : IEntityTypeConfiguration<GameModeEntity>
{
    public void Configure(EntityTypeBuilder<GameModeEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}