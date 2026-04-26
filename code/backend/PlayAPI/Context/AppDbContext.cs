using Microsoft.EntityFrameworkCore;
using PlayAPI.Data;

namespace PlayAPI.Context;

public partial class AppDbContext : DbContext
{
    public DbSet<GameEvent> GameEvents => Set<GameEvent>();
    public DbSet<GameMetric> GameMetrics => Set<GameMetric>();

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
