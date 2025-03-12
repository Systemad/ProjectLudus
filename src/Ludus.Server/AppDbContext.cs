using Ludus.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ludus.Server;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<LudusUser> Users { get; set; }
    public DbSet<LudusUserImage> Images { get; set; }
    
    private static readonly IgnoringIdentityResolutionInterceptor IgnoringIdentityResolutionInterceptor =
        new();
    
    public string DbPath { get; }
    public AppDbContext()
    {
        var projectRoot = Directory.GetCurrentDirectory();
        DbPath = Path.Combine(projectRoot, "testing.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.AddInterceptors(IgnoringIdentityResolutionInterceptor);
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){}
}