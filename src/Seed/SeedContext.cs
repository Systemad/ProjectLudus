using Microsoft.EntityFrameworkCore;
using Shared;

namespace Seed;

public class SeedContext : DbContext
{
    public string DbPath { get; }
    public DbSet<AgeRatingOrganizations> AgeRatingOrganizations { get; set; }
    public DbSet<AgeRatingCategories> AgeRatingCategories { get; set; }
    public DbSet<AgeRatingContentDescriptionsV2> AgeRatingContentDescriptionsV2 { get; set; }
    public DbSet<AgeRatings> AgeRatings { get; set; }
    public DbSet<CharacterGenders> CharacterGenders { get; set; }
    public DbSet<CharacterSpecies> CharacterSpecies { get; set; }
    public DbSet<CollectionTypes> CollectionTypes { get; set; }
    public DbSet<CompanyStatuses> CompanyStatuses { get; set; }
    public DbSet<ExternalGameSources> ExternalGameSources { get; set; }
    public DbSet<GameEngineLogos> GameEngineLogos { get; set; }
    public DbSet<GameModes> GameModes { get; set; }
    public DbSet<GameReleaseFormats> GameReleaseFormats { get; set; }
    public DbSet<GameStatuses> GameStatuses { get; set; }
    public DbSet<GameTypes> GameTypes { get; set; }
    public DbSet<Genres> Genres { get; set; }
    public DbSet<Languages> Languages { get; set; }
    public DbSet<PlatformTypes> PlatformTypes { get; set; }
    public DbSet<PlatformWebsites> PlatformWebsites { get; set; }
    public DbSet<PlayerPerspectives> PlayerPerspectives { get; set; }
    public DbSet<Regions> Regions { get; set; }
    public DbSet<ReleaseDateRegions> ReleaseDateRegions { get; set; }
    public DbSet<ReleaseDateStatuses> ReleaseDateStatuses { get; set; }
    public DbSet<Themes> Themes { get; set; }
    public DbSet<DateFormats> DateFormats { get; set; }
    public DbSet<WebsiteTypes> WebsiteTypes { get; set; }
    public DbSet<LanguageSupportTypes> LanguageSupportTypes { get; set; }
    public DbSet<PlatformLogos> PlatformLogos { get; set; }
    public DbSet<PlatformFamilies> PlatformFamilies { get; set; }

    public SeedContext()
    {
        DbPath = Path.Combine(Directory.GetCurrentDirectory(), "seeded.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}