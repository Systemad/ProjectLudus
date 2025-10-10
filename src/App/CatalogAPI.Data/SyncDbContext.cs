using CatalogAPI.Data.Entities;
using CatalogAPI.Data.Features.Companies;
using CatalogAPI.Data.Features.GameEngines;
using CatalogAPI.Data.Features.GameModes;
using CatalogAPI.Data.Features.Genres;
using CatalogAPI.Data.Features.Platforms;
using CatalogAPI.Data.Features.Themes;
using Microsoft.EntityFrameworkCore;
using Shared.Features.Games;
using Shared.Features.IGDB;
using Shared.Features.PopScore;
using Shared.Features.References.Platform;
using Shared.Features.Webhooks;

namespace CatalogAPI.Data;

public class SyncDbContext(DbContextOptions<SyncDbContext> options) : DbContext(options)
{
    public DbSet<PopScoreGame> PopScoreGames { get; set; }
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<CompanyEntity> Companies { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }
    public DbSet<PlatformEntity> Platforms { get; set; }
    public DbSet<GameEngineEntity> GameEngines { get; set; }
    public DbSet<ThemeEntity> Themes { get; set; }
    public DbSet<FranchiseEntity> Franchises { get; set; }
    public DbSet<GameModeEntity> GameModes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasPostgresExtension("pg_search");
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresExtension("pg_ivm");
        modelBuilder.HasPostgresExtension("pg_cron");


        /*
        modelBuilder.Entity<GameEntity>()
            .OwnsOne(c => c.RawData, d =>
            {
                d.ToJson();
            });
        */
        /*
        modelBuilder.Entity<GameEntity>(b =>
        {
            b.Property(g => g.Id).ValueGeneratedNever();
            b.ComplexProperty(g => g.Metadata);
        });
        */



    }
}