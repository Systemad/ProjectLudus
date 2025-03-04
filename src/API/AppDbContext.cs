using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared;

namespace API;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    
    private static readonly IgnoringIdentityResolutionInterceptor IgnoringIdentityResolutionInterceptor =
        new();
    
    public string DbPath { get; }
    public AppDbContext()
    {
        var projectRoot = Directory.GetCurrentDirectory();
        DbPath = Path.Combine(projectRoot, "games.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.AddInterceptors(IgnoringIdentityResolutionInterceptor);
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){}
}