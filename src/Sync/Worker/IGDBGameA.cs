using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/*
namespace Worker
{
    public class IGDBGame
    {
        [Key]
        public long Id { get; set; }

        public long CreatedAt { get; set; }
        public long FirstReleaseDate { get; set; }
        public long? Hypes { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public long RatingCount { get; set; }
        public string Slug { get; set; }
        public string Storyline { get; set; }
        public string Summary { get; set; }
        public double TotalRating { get; set; }
        public long TotalRatingCount { get; set; }
        public long UpdatedAt { get; set; }
        public string Url { get; set; }
        public string Checksum { get; set; }

        // Navigation collections
        public ICollection<AgeRating> AgeRatings { get; set; }
        public ICollection<AlternativeName> AlternativeNames { get; set; }
        public ICollection<Artwork> Artworks { get; set; }
        public Cover Cover { get; set; }

        public ICollection<Dlc> Dlcs { get; set; }
        public ICollection<Expansion> Expansions { get; set; }
        public ICollection<Franchise> Franchises { get; set; }
        public ICollection<GameEngine> GameEngines { get; set; }
        public ICollection<GameMode> GameModes { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<InvolvedCompany> InvolvedCompanies { get; set; }
        public ICollection<Keyword> Keywords { get; set; }
        public ICollection<Platform> Platforms { get; set; }
        public ICollection<PlayerPerspective> PlayerPerspectives { get; set; }
        public ICollection<ReleaseDate> ReleaseDates { get; set; }
        public ICollection<Screenshot> Screenshots { get; set; }
        public ICollection<SimilarGame> SimilarGames { get; set; }
        public ICollection<Theme> Themes { get; set; }
        public ICollection<Video> Videos { get; set; }
        public ICollection<GameWebsite> Websites { get; set; }
        public ICollection<LanguageSupport> LanguageSupports { get; set; }
        public ICollection<Collection> Collections { get; set; }

        public GameType GameType { get; set; }
        public ICollection<MultiplayerMode> MultiplayerModes { get; set; }
    }

    public class AgeRating
    {
        [Key]
        public int Id { get; set; }
        public long Rating { get; set; }
        public string Category { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class AlternativeName
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Artwork
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Cover
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }

        [ForeignKey(nameof(IGDBGame))]
        public long IGDBGameId { get; set; }
        public IGDBGame IGDBGame { get; set; }
    }

    public class Dlc
    {
        [Key]
        public int Id { get; set; }
        public long DlcGameId { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Expansion
    {
        [Key]
        public int Id { get; set; }
        public long ExpansionGameId { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Franchise
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class GameEngine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class GameMode
    {
        [Key]
        public int Id { get; set; }
        public string Mode { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class InvolvedCompany
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        public string KeywordText { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Platform
    {
        [Key]
        public int Id { get; set; }
        public string PlatformName { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class PlayerPerspective
    {
        [Key]
        public int Id { get; set; }
        public string Perspective { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class ReleaseDate
    {
        [Key]
        public int Id { get; set; }
        public long Date { get; set; }
        public string Region { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Screenshot
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class SimilarGame
    {
        [Key]
        public int Id { get; set; }
        public long SimilarGameId { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Theme
    {
        [Key]
        public int Id { get; set; }
        public string ThemeName { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string VideoUrl { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class GameWebsite
    {
        [Key]
        public int Id { get; set; }
        public string WebsiteUrl { get; set; }
        public string Category { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class LanguageSupport
    {
        [Key]
        public int Id { get; set; }
        public string Language { get; set; }
        public bool IsSupported { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class Collection
    {
        [Key]
        public int Id { get; set; }
        public string CollectionName { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    public class GameType
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
    }

    public class MultiplayerMode
    {
        [Key]
        public int Id { get; set; }
        public string ModeName { get; set; }

        public long IGDBGameId { get; set; }
        [ForeignKey(nameof(IGDBGameId))]
        public IGDBGame IGDBGame { get; set; }
    }

    // DbContext
    public class GamesDbContext : DbContext
    {
        public GamesDbContext(DbContextOptions<GamesDbContext> options) : base(options)
        {
        }

        public DbSet<IGDBGame> IGDBGames { get; set; }
        public DbSet<AgeRating> AgeRatings { get; set; }
        public DbSet<AlternativeName> AlternativeNames { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<Dlc> Dlcs { get; set; }
        public DbSet<Expansion> Expansions { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<GameEngine> GameEngines { get; set; }
        public DbSet<GameMode> GameModes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<InvolvedCompany> InvolvedCompanies { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<PlayerPerspective> PlayerPerspectives { get; set; }
        public DbSet<ReleaseDate> ReleaseDates { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }
        public DbSet<SimilarGame> SimilarGames { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<GameWebsite> Websites { get; set; }
        public DbSet<LanguageSupport> LanguageSupports { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<MultiplayerMode> MultiplayerModes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one for Cover
            modelBuilder.Entity<IGDBGame>()
                .HasOne(g => g.Cover)
                .WithOne(c => c.IGDBGame)
                .HasForeignKey<Cover>(c => c.IGDBGameId);

            // Configure one-to-many relationships
            modelBuilder.Entity<AgeRating>()
                .HasOne(ar => ar.IGDBGame)
                .WithMany(g => g.AgeRatings)
                .HasForeignKey(ar => ar.IGDBGameId);

            modelBuilder.Entity<AlternativeName>()
                .HasOne(an => an.IGDBGame)
                .WithMany(g => g.AlternativeNames)
                .HasForeignKey(an => an.IGDBGameId);

            // Repeat for all collections similarly:
            modelBuilder.Entity<Artwork>()
                .HasOne(a => a.IGDBGame)
                .WithMany(g => g.Artworks)
                .HasForeignKey(a => a.IGDBGameId);

            // ... Continue for all other collections
            modelBuilder.Entity<Dlc>()
                .HasOne(d => d.IGDBGame)
                .WithMany(g => g.Dlcs)
                .HasForeignKey(d => d.IGDBGameId);

            modelBuilder.Entity<Expansion>()
                .HasOne(e => e.IGDBGame)
                .WithMany(g => g.Expansions)
                .HasForeignKey(e => e.IGDBGameId);

            // ... (repeat for others)

            base.OnModelCreating(modelBuilder);
        }
    }
}
*/