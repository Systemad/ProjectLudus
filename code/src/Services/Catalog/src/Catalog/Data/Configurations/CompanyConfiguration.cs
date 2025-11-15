using Catalog.Companies.Models;
using Catalog.Games.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.Property(g => g.Id).ValueGeneratedNever();

        /*
        builder
            .HasMany(c => c.Developed)
            .WithMany(g => g.Developers)
            .UsingEntity(
                "developer_game",
                r => r.HasOne(typeof(GameEntity)).WithMany().HasForeignKey("game_id"),
                l => l.HasOne(typeof(CompanyEntity)).WithMany().HasForeignKey("company_id")
            );

        builder
            .HasMany(c => c.Published)
            .WithMany(g => g.Publishers)
            .UsingEntity(
                "publisher_game",
                r => r.HasOne(typeof(GameEntity)).WithMany().HasForeignKey("game_id"),
                l => l.HasOne(typeof(CompanyEntity)).WithMany().HasForeignKey("company_id")
            );
*/
        builder
            .HasOne(c => c.Parent)
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.ChangedCompany)
            .WithMany()
            .HasForeignKey(c => c.ChangedCompanyId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.Metadata).HasColumnType("jsonb");
    }
}
