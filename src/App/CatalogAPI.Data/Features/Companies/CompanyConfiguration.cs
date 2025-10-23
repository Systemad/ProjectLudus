using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.Companies;

public class CompanyConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();

        builder
            .HasMany(c => c.Developed)
            .WithMany(g => g.Developers)
            // Optional: Name the join table for clarity
            .UsingEntity(j => j.ToTable("DeveloperGame"));

        builder
            .HasMany(c => c.Published)
            .WithMany(g => g.Publishers)
            .UsingEntity(j => j.ToTable("PublisherGame"));
        
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
        //builder.ComplexProperty<Company>(b => b.Metadata, d => d.ToJson()); 
