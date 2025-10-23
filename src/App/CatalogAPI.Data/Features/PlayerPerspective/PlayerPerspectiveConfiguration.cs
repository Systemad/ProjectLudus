using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Features.PlayerPerspective;

public class PlayerPerspectiveConfiguration : IEntityTypeConfiguration<PlayerPerspectiveEntity>
{
    public void Configure(EntityTypeBuilder<PlayerPerspectiveEntity> builder)
    {
        builder
            .Property(g => g.Id)
            .ValueGeneratedNever();
    }
}
