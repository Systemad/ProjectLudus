using Ludus.Shared.Features.User;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Server;

public class AppDbContext : DbContext
{
    public DbSet<LudusUser> Users { get; set; }
    public DbSet<UserGameStatus> UserGameStatus { get; set; }
    public DbSet<LudusUserImage> Images { get; set; }

    public string DbPath { get; }

    public AppDbContext()
    {
        var projectRoot = Directory.GetCurrentDirectory();
        DbPath = Path.Combine(projectRoot, "testing.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}
