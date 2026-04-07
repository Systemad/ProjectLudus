using Microsoft.EntityFrameworkCore;
using PlayAPI.Data;

namespace PlayAPI.Context;

public partial class AppDbContext : DbContext
{
    public DbSet<GameVisitCount> GameVisitCounts => Set<GameVisitCount>();
    public DbSet<TypesenseKey> TypesenseKeys => Set<TypesenseKey>();

    private static readonly DateTime MockGameVisitTimestamp = new(
        2026,
        1,
        1,
        0,
        0,
        0,
        DateTimeKind.Utc
    );

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<GameVisitCount>()
            .HasData(
                new GameVisitCount
                {
                    Id = 1,
                    GameId = -1,
                    Count = 0,
                    LastVisitedAt = MockGameVisitTimestamp,
                }
            );
    }
}
