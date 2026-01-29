using Catalog.Companies.Models;
using Catalog.Games.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder.Property(g => g.Id).ValueGeneratedNever();
        
        builder
            .HasMany(c => c.Developers)
            .WithMany(g => g.Developed)
            .UsingEntity(
                "developer_game",
                r => r.HasOne(typeof(CompanyEntity)).WithMany().HasForeignKey("company_id"),
                l => l.HasOne(typeof(GameEntity)).WithMany().HasForeignKey("game_id")
            );

        builder
            .HasMany(g => g.Publishers)
            .WithMany(c => c.Published)
            .UsingEntity(
                "publisher_game",
                r => r.HasOne(typeof(CompanyEntity)).WithMany().HasForeignKey("company_id"),
                l => l.HasOne(typeof(GameEntity)).WithMany().HasForeignKey("game_id")
            );
        builder.Property(c => c.Metadata).HasColumnType("jsonb");
    }
}
