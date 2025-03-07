using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared;

namespace API;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }  

    private static readonly IgnoringIdentityResolutionInterceptor IgnoringIdentityResolutionInterceptor =
        new();


    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(IgnoringIdentityResolutionInterceptor);
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=gamesdb;Username=postgres;Password=Compaq2009")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}