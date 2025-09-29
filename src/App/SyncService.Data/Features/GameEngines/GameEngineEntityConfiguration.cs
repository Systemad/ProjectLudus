using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Features.Games;

namespace SyncService.Data.Features.GameEngines;

public class GameEngineEntityConfiguration : IEntityTypeConfiguration<GameEngineEntity>
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

public class RawGameEngineEntityConfiguration : IEntityTypeConfiguration<RawGameEngineEntity>
{
    public void Configure(EntityTypeBuilder<RawGameEngineEntity> builder)
    {
        builder
            .OwnsOne(c => c.Payload, d => { d.ToJson(); })
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}