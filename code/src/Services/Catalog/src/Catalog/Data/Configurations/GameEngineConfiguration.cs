using Catalog.GameEngines;
using Catalog.GameEngines.Models;
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

        builder.Property(c => c.Metadata).HasColumnType("jsonb");
        builder.Property(c => c.Logo).HasColumnType("jsonb");
        
        //builder.ComplexProperty<GameEngine>(b => b.Metadata, d => d.ToJson());
        //builder.ComplexProperty<GameEngineLogo>(b => b.Logo, d => d.ToJson());
    }
}
