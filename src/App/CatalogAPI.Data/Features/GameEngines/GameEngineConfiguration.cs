using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.GameEngines;

public class GameEngineConfiguration : IEntityTypeConfiguration<GameEngineEntity>
{
    public void Configure(EntityTypeBuilder<GameEngineEntity> builder)
    {
        builder.Property(g => g.Id)
            .ValueGeneratedNever();
        
        builder.ComplexProperty<GameEngineLogo>(b => b.Logo)
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}
