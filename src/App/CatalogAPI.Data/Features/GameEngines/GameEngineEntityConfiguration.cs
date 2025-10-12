using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Features.Games;
using Shared.Features.IGDB;

namespace CatalogAPI.Data.Features.GameEngines;

public class GameEngineEntityConfiguration : IEntityTypeConfiguration<GameEngineEntity>
{
    public void Configure(EntityTypeBuilder<GameEngineEntity> builder)
    {
        builder.Property(g => g.Id)
            .ValueGeneratedNever();
        
        builder.ComplexProperty<IgdbGameEngineLogo>(b => b.Logo)
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}
