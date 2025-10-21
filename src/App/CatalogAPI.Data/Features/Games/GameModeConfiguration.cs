using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.Games;

public class GameConfiguration : IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
        
        builder.ComplexProperty<Game>(b => b.Metadata);
    }
}