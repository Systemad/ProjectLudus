using CatalogAPI.Data.Features.Companies;
using CatalogAPI.Data.Features.Franchises;
using CatalogAPI.Data.Features.GameEngines;
using CatalogAPI.Data.Features.GameModes;
using CatalogAPI.Data.Features.Games;
using CatalogAPI.Data.Features.Genres;
using CatalogAPI.Data.Features.Platforms;
using CatalogAPI.Data.Features.PlayerPerspective;
using CatalogAPI.Data.Features.Themes;
using Microsoft.EntityFrameworkCore;


namespace CatalogAPI.Data;

public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
{
    //public DbSet<PopularityPrimitive> PopScoreGames { get; set; }
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<CompanyEntity> Companies { get; set; }
    public DbSet<FranchiseEntity> Franchises { get; set; }
    public DbSet<GameEngineEntity> GameEngines { get; set; }
    public DbSet<GameModeEntity> GameModes { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }
    public DbSet<PlatformEntity> Platforms { get; set; }
    public DbSet<ThemeEntity> Themes { get; set; }
    public DbSet<PlayerPerspectiveEntity> PlayerPerspectives { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasPostgresExtension("pg_search");
        //modelBuilder.HasPostgresExtension("vector");
        //modelBuilder.HasPostgresExtension("postgis");
        //modelBuilder.HasPostgresExtension("pg_ivm");
        //modelBuilder.HasPostgresExtension("pg_cron");
        
        modelBuilder.ApplyConfiguration(new GameModeConfiguration());
        modelBuilder.ApplyConfiguration(new GameEngineConfiguration());
        modelBuilder.ApplyConfiguration(new FranchiseConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new ThemeConfiguration());
        modelBuilder.ApplyConfiguration(new PlatformConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerPerspectiveConfiguration());
        
        modelBuilder.ApplyConfiguration(new GameConfiguration());
    }
}

/*
public class PostDto
{
    public int Id { get; set; }
    public List<int> TagIds { get; set; }
}
var postsJson = JsonSerializer.Deserialize<List<PostDto>>(jsonString);

var joinPairs = new List<(int PostId, int TagId)>();
foreach (var post in postsJson)
{
    foreach (var tagId in post.TagIds)
    {
        joinPairs.Add((post.Id, tagId));
    }
}


using var conn = new NpgsqlConnection(connectionString);
await conn.OpenAsync();

using var writer = conn.BeginBinaryImport("COPY \"PostTag\" (\"PostsId\", \"TagsId\") FROM STDIN (FORMAT BINARY)");
foreach (var pair in joinPairs)
{
    writer.StartRow();
    writer.Write(pair.PostId, NpgsqlTypes.NpgsqlDbType.Integer);
    writer.Write(pair.TagId, NpgsqlTypes.NpgsqlDbType.Integer);
}
await writer.CompleteAsync();


*/