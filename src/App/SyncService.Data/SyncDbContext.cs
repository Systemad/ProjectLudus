using Microsoft.EntityFrameworkCore;
using Shared.Features;
using Shared.Features.Games;
using Shared.Features.PopScore;
using Shared.Features.Webhooks;

namespace SyncService.Data;

public class SyncDbContext(DbContextOptions<SyncDbContext> options) : DbContext(options)
{
    public DbSet<ActiveWebhook> ActiveWebhooks { get; set; }
    public DbSet<PopScoreGame> PopScoreGames { get; set; }
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<CompanyEntity> Companies { get; set; }
    public DbSet<GameMode> GameModes { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<PlayerPerspective> PlayerPerspectives { get; set; }
    public DbSet<GameEngine> GameEngines { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<Franchise> Franchises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasPostgresExtension("pg_search");
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresExtension("pg_ivm");
        modelBuilder.HasPostgresExtension("pg_cron");
        
        modelBuilder.Entity<PopScoreGame>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<GameEntity>()
            .Property(g => g.Id)
            .ValueGeneratedNever();
        /*
        modelBuilder.Entity<GameEntity>()
            .OwnsOne(c => c.RawData, d =>
            {
                d.ToJson();
            });
        */
        modelBuilder.Entity<GameEntity>().Property(g => g.RawData).HasColumnType("jsonb");

        modelBuilder.Entity<GameMode>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Genre>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Platform>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<PlayerPerspective>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<GameEngine>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Theme>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Keyword>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Franchise>()
            .Property(g => g.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Platform>()
            .Property(p => p.PlatformLogo)
            .HasColumnType("jsonb");

        modelBuilder.Entity<GameEngine>()
            .Property(p => p.Logo)
            .HasColumnType("jsonb");

    }
}