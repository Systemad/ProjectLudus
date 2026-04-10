using System;
using System.Collections.Generic;
using CatalogAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgeRating> AgeRatings { get; set; }

    public virtual DbSet<AgeRatingCategory> AgeRatingCategories { get; set; }

    public virtual DbSet<AgeRatingContentDescriptionType> AgeRatingContentDescriptionTypes { get; set; }

    public virtual DbSet<AgeRatingContentDescriptionsV2> AgeRatingContentDescriptionsV2s { get; set; }

    public virtual DbSet<AgeRatingOrganization> AgeRatingOrganizations { get; set; }

    public virtual DbSet<AlternativeName> AlternativeNames { get; set; }

    public virtual DbSet<Artwork> Artworks { get; set; }

    public virtual DbSet<ArtworkType> ArtworkTypes { get; set; }

    public virtual DbSet<BridgeGameGameEngine> BridgeGameGameEngines { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterGender> CharacterGenders { get; set; }

    public virtual DbSet<CharacterMugShot> CharacterMugShots { get; set; }

    public virtual DbSet<CharacterSpecy> CharacterSpecies { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyLogo> CompanyLogos { get; set; }

    public virtual DbSet<CompanySearch> CompanySearches { get; set; }

    public virtual DbSet<CompanyStatus> CompanyStatuses { get; set; }

    public virtual DbSet<CompanyWebsite> CompanyWebsites { get; set; }

    public virtual DbSet<Cover> Covers { get; set; }

    public virtual DbSet<DateFormat> DateFormats { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventLogo> EventLogos { get; set; }

    public virtual DbSet<EventNetwork> EventNetworks { get; set; }

    public virtual DbSet<ExternalGame> ExternalGames { get; set; }

    public virtual DbSet<ExternalGameSource> ExternalGameSources { get; set; }

    public virtual DbSet<Franchise> Franchises { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameEngine> GameEngines { get; set; }

    public virtual DbSet<GameEngineLogo> GameEngineLogos { get; set; }

    public virtual DbSet<GameGameEngine> GameGameEngines { get; set; }

    public virtual DbSet<GameLocalization> GameLocalizations { get; set; }

    public virtual DbSet<GameMode> GameModes { get; set; }

    public virtual DbSet<GamePopularityLatest> GamePopularityLatests { get; set; }

    public virtual DbSet<GameReleaseFormat> GameReleaseFormats { get; set; }

    public virtual DbSet<GameStatus> GameStatuses { get; set; }

    public virtual DbSet<GameType> GameTypes { get; set; }

    public virtual DbSet<GamesSearch> GamesSearches { get; set; }

    public virtual DbSet<GamesSimilarGame> GamesSimilarGames { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<InvolvedCompany> InvolvedCompanies { get; set; }

    public virtual DbSet<Keyword> Keywords { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LanguageSupportType> LanguageSupportTypes { get; set; }

    public virtual DbSet<MartCompanySearch> MartCompanySearches { get; set; }

    public virtual DbSet<MartGamesSearch> MartGamesSearches { get; set; }

    public virtual DbSet<MartGamesSearchPopularityTest> MartGamesSearchPopularityTests { get; set; }

    public virtual DbSet<MultiplayerMode> MultiplayerModes { get; set; }

    public virtual DbSet<NetworkType> NetworkTypes { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<PlatformFamily> PlatformFamilies { get; set; }

    public virtual DbSet<PlatformLogo> PlatformLogos { get; set; }

    public virtual DbSet<PlatformType> PlatformTypes { get; set; }

    public virtual DbSet<PlatformVersion> PlatformVersions { get; set; }

    public virtual DbSet<PlatformVersionCompany1> PlatformVersionCompanies1 { get; set; }

    public virtual DbSet<PlatformWebsite> PlatformWebsites { get; set; }

    public virtual DbSet<PlayerPerspective> PlayerPerspectives { get; set; }

    public virtual DbSet<PopularityPrimitive> PopularityPrimitives { get; set; }

    public virtual DbSet<PopularityType> PopularityTypes { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<ReleaseDate> ReleaseDates { get; set; }

    public virtual DbSet<ReleaseDateRegion> ReleaseDateRegions { get; set; }

    public virtual DbSet<ReleaseDateStatus> ReleaseDateStatuses { get; set; }

    public virtual DbSet<Screenshot> Screenshots { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<Website> Websites { get; set; }

    public virtual DbSet<WebsiteType> WebsiteTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Username=postgres;Password=pudGW.E7_u8eF8Qhnym)E0;Database=catalogdev", x => x.UseNodaTime());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgeRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("age_ratings__dbt_tmp_pkey");

            entity.ToTable("age_ratings");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Organization).HasColumnName("organization");
            entity.Property(e => e.RatingCategory).HasColumnName("rating_category");
            entity.Property(e => e.RatingContentDescriptions)
                .HasColumnType("jsonb")
                .HasColumnName("rating_content_descriptions");
            entity.Property(e => e.Synopsis)
                .HasColumnType("character varying")
                .HasColumnName("synopsis");

            entity.HasOne(d => d.OrganizationNavigation).WithMany(p => p.AgeRatings)
                .HasForeignKey(d => d.Organization)
                .HasConstraintName("age_ratings__dbt_tmp_organization_fkey");

            entity.HasOne(d => d.RatingCategoryNavigation).WithMany(p => p.AgeRatings)
                .HasForeignKey(d => d.RatingCategory)
                .HasConstraintName("age_ratings__dbt_tmp_rating_category_fkey");
        });

        modelBuilder.Entity<AgeRatingCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("age_rating_categories__dbt_tmp_pkey");

            entity.ToTable("age_rating_categories");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Organization).HasColumnName("organization");
            entity.Property(e => e.Rating)
                .HasColumnType("character varying")
                .HasColumnName("rating");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.OrganizationNavigation).WithMany(p => p.AgeRatingCategories)
                .HasForeignKey(d => d.Organization)
                .HasConstraintName("age_rating_categories__dbt_tmp_organization_fkey");
        });

        modelBuilder.Entity<AgeRatingContentDescriptionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("age_rating_content_description_types__dbt_tmp_pkey");

            entity.ToTable("age_rating_content_description_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<AgeRatingContentDescriptionsV2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("age_rating_content_descriptions_v2__dbt_tmp_pkey");

            entity.ToTable("age_rating_content_descriptions_v2");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.DescriptionType).HasColumnName("description_type");
            entity.Property(e => e.Organization).HasColumnName("organization");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.DescriptionTypeNavigation).WithMany(p => p.AgeRatingContentDescriptionsV2s)
                .HasForeignKey(d => d.DescriptionType)
                .HasConstraintName("age_rating_content_descriptions_v2__dbt_t_description_type_fkey");

            entity.HasOne(d => d.OrganizationNavigation).WithMany(p => p.AgeRatingContentDescriptionsV2s)
                .HasForeignKey(d => d.Organization)
                .HasConstraintName("age_rating_content_descriptions_v2__dbt_tmp_organization_fkey");
        });

        modelBuilder.Entity<AgeRatingOrganization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("age_rating_organizations__dbt_tmp_pkey");

            entity.ToTable("age_rating_organizations");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<AlternativeName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("alternative_names__dbt_tmp_pkey");

            entity.ToTable("alternative_names");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.AlternativeNames)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("alternative_names__dbt_tmp_game_fkey");
        });

        modelBuilder.Entity<Artwork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artworks__dbt_tmp_pkey");

            entity.ToTable("artworks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<ArtworkType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artwork_types__dbt_tmp_pkey");

            entity.ToTable("artwork_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<BridgeGameGameEngine>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("bridge_game__game_engine");

            entity.Property(e => e.GameEngineId).HasColumnName("game_engine_id");
            entity.Property(e => e.GameId).HasColumnName("game_id");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("characters__dbt_tmp_pkey");

            entity.ToTable("characters");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Akas)
                .HasColumnType("jsonb")
                .HasColumnName("akas");
            entity.Property(e => e.CharacterGender).HasColumnName("character_gender");
            entity.Property(e => e.CharacterSpecies).HasColumnName("character_species");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Games)
                .HasColumnType("jsonb")
                .HasColumnName("games");
            entity.Property(e => e.MugShot).HasColumnName("mug_shot");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.CharacterGenderNavigation).WithMany(p => p.Characters)
                .HasForeignKey(d => d.CharacterGender)
                .HasConstraintName("characters__dbt_tmp_character_gender_fkey");

            entity.HasOne(d => d.CharacterSpeciesNavigation).WithMany(p => p.Characters)
                .HasForeignKey(d => d.CharacterSpecies)
                .HasConstraintName("characters__dbt_tmp_character_species_fkey");

            entity.HasOne(d => d.MugShotNavigation).WithMany(p => p.Characters)
                .HasForeignKey(d => d.MugShot)
                .HasConstraintName("characters__dbt_tmp_mug_shot_fkey");
        });

        modelBuilder.Entity<CharacterGender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("character_genders__dbt_tmp_pkey");

            entity.ToTable("character_genders");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<CharacterMugShot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("character_mug_shots__dbt_tmp_pkey");

            entity.ToTable("character_mug_shots");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<CharacterSpecy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("character_species__dbt_tmp_pkey");

            entity.ToTable("character_species");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companies__dbt_tmp_pkey");

            entity.ToTable("companies");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ChangeDate).HasColumnName("change_date");
            entity.Property(e => e.ChangeDateCategory).HasColumnName("change_date_category");
            entity.Property(e => e.ChangedCompanyId).HasColumnName("changed_company_id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StartDateFormat).HasColumnName("start_date_format");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.ChangedCompany).WithMany(p => p.InverseChangedCompany)
                .HasForeignKey(d => d.ChangedCompanyId)
                .HasConstraintName("companies_changed_company_id_fkey");

            entity.HasOne(d => d.LogoNavigation).WithMany(p => p.Companies)
                .HasForeignKey(d => d.Logo)
                .HasConstraintName("companies__dbt_tmp_logo_fkey");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("companies_parent_id_fkey");

            entity.HasOne(d => d.StartDateFormatNavigation).WithMany(p => p.Companies)
                .HasForeignKey(d => d.StartDateFormat)
                .HasConstraintName("companies__dbt_tmp_start_date_format_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Companies)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("companies__dbt_tmp_status_fkey");

            entity.HasMany(d => d.PlatformVersions).WithMany(p => p.Companies)
                .UsingEntity<Dictionary<string, object>>(
                    "PlatformVersionCompany",
                    r => r.HasOne<PlatformVersion>().WithMany()
                        .HasForeignKey("PlatformVersionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("platform_version_company__dbt_tmp_platform_version_id_fkey"),
                    l => l.HasOne<Company>().WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("platform_version_company__dbt_tmp_company_id_fkey"),
                    j =>
                    {
                        j.HasKey("CompanyId", "PlatformVersionId").HasName("platform_version_company__dbt_tmp_pkey");
                        j.ToTable("platform_version_company");
                        j.IndexerProperty<long>("CompanyId").HasColumnName("company_id");
                        j.IndexerProperty<long>("PlatformVersionId").HasColumnName("platform_version_id");
                    });
        });

        modelBuilder.Entity<CompanyLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_logos__dbt_tmp_pkey");

            entity.ToTable("company_logos");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<CompanySearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("company_search", tb => tb.HasComment("Search-ready companies dataset for Typesense indexing"));

            entity.Property(e => e.ChangedCompany)
                .HasColumnType("character varying")
                .HasColumnName("changed_company");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.GamesDevelopedCount).HasColumnName("games_developed_count");
            entity.Property(e => e.GamesPublishedCount).HasColumnName("games_published_count");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogoUrl)
                .HasColumnType("character varying")
                .HasColumnName("logo_url");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.ParentCompany)
                .HasColumnType("character varying")
                .HasColumnName("parent_company");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StartYear).HasColumnName("start_year");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<CompanyStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_statuses__dbt_tmp_pkey");

            entity.ToTable("company_statuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<CompanyWebsite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_websites__dbt_tmp_pkey");

            entity.ToTable("company_websites");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Trusted).HasColumnName("trusted");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.CompanyWebsites)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("company_websites__dbt_tmp_type_fkey");
        });

        modelBuilder.Entity<Cover>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("covers__dbt_tmp_pkey");

            entity.ToTable("covers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<DateFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("date_formats__dbt_tmp_pkey");

            entity.ToTable("date_formats");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Format)
                .HasColumnType("character varying")
                .HasColumnName("format");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events__dbt_tmp_pkey");

            entity.ToTable("events");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.EventLogo).HasColumnName("event_logo");
            entity.Property(e => e.EventNetworks)
                .HasColumnType("jsonb")
                .HasColumnName("event_networks");
            entity.Property(e => e.Games)
                .HasColumnType("jsonb")
                .HasColumnName("games");
            entity.Property(e => e.LiveStreamUrl)
                .HasColumnType("character varying")
                .HasColumnName("live_stream_url");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.TimeZone)
                .HasColumnType("character varying")
                .HasColumnName("time_zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Videos)
                .HasColumnType("jsonb")
                .HasColumnName("videos");

            entity.HasOne(d => d.EventLogoNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventLogo)
                .HasConstraintName("events__dbt_tmp_event_logo_fkey");
        });

        modelBuilder.Entity<EventLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_logos__dbt_tmp_pkey");

            entity.ToTable("event_logos");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Event).HasColumnName("event");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<EventNetwork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_networks__dbt_tmp_pkey");

            entity.ToTable("event_networks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Event).HasColumnName("event");
            entity.Property(e => e.NetworkType).HasColumnName("network_type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.EventNavigation).WithMany(p => p.EventNetworksNavigation)
                .HasForeignKey(d => d.Event)
                .HasConstraintName("event_networks__dbt_tmp_event_fkey");

            entity.HasOne(d => d.NetworkTypeNavigation).WithMany(p => p.EventNetworksNavigation)
                .HasForeignKey(d => d.NetworkType)
                .HasConstraintName("event_networks__dbt_tmp_network_type_fkey");
        });

        modelBuilder.Entity<ExternalGame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("external_games__dbt_tmp_pkey1");

            entity.ToTable("external_games");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Countries)
                .HasColumnType("jsonb")
                .HasColumnName("countries");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExternalGameSource).HasColumnName("external_game_source");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.GameReleaseFormat).HasColumnName("game_release_format");
            entity.Property(e => e.Media).HasColumnName("media");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Platform).HasColumnName("platform");
            entity.Property(e => e.Uid)
                .HasColumnType("character varying")
                .HasColumnName("uid");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.ExternalGameSourceNavigation).WithMany(p => p.ExternalGames)
                .HasForeignKey(d => d.ExternalGameSource)
                .HasConstraintName("external_games__dbt_tmp_external_game_source_fkey");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.ExternalGames)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("external_games__dbt_tmp_game_fkey");

            entity.HasOne(d => d.GameReleaseFormatNavigation).WithMany(p => p.ExternalGames)
                .HasForeignKey(d => d.GameReleaseFormat)
                .HasConstraintName("external_games__dbt_tmp_game_release_format_fkey");

            entity.HasOne(d => d.PlatformNavigation).WithMany(p => p.ExternalGames)
                .HasForeignKey(d => d.Platform)
                .HasConstraintName("external_games__dbt_tmp_platform_fkey");
        });

        modelBuilder.Entity<ExternalGameSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("external_game_sources__dbt_tmp_pkey");

            entity.ToTable("external_game_sources");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Franchise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("franchises__dbt_tmp_pkey");

            entity.ToTable("franchises");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Games)
                .HasColumnType("jsonb")
                .HasColumnName("games");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games__dbt_tmp_pkey");

            entity.ToTable("games");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AggregatedRating).HasColumnName("aggregated_rating");
            entity.Property(e => e.AggregatedRatingCount).HasColumnName("aggregated_rating_count");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Cover).HasColumnName("cover");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FirstReleaseDate).HasColumnName("first_release_date");
            entity.Property(e => e.Franchise).HasColumnName("franchise");
            entity.Property(e => e.GameStatus).HasColumnName("game_status");
            entity.Property(e => e.GameType).HasColumnName("game_type");
            entity.Property(e => e.Hypes).HasColumnName("hypes");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.ParentGame).HasColumnName("parent_game");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RatingCount).HasColumnName("rating_count");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Storyline)
                .HasColumnType("character varying")
                .HasColumnName("storyline");
            entity.Property(e => e.Summary)
                .HasColumnType("character varying")
                .HasColumnName("summary");
            entity.Property(e => e.TotalRating).HasColumnName("total_rating");
            entity.Property(e => e.TotalRatingCount).HasColumnName("total_rating_count");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.VersionParent).HasColumnName("version_parent");
            entity.Property(e => e.VersionTitle)
                .HasColumnType("character varying")
                .HasColumnName("version_title");

            entity.HasOne(d => d.CoverNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.Cover)
                .HasConstraintName("games__dbt_tmp_cover_fkey");

            entity.HasOne(d => d.FranchiseNavigation).WithMany(p => p.GamesNavigation)
                .HasForeignKey(d => d.Franchise)
                .HasConstraintName("games__dbt_tmp_franchise_fkey");

            entity.HasOne(d => d.GameStatusNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameStatus)
                .HasConstraintName("games__dbt_tmp_game_status_fkey");

            entity.HasOne(d => d.GameTypeNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameType)
                .HasConstraintName("games__dbt_tmp_game_type_fkey");

            entity.HasMany(d => d.BaseGames).WithMany(p => p.DlcGames)
                .UsingEntity<Dictionary<string, object>>(
                    "GamesDlc",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("BaseGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("games_dlc__dbt_tmp_base_game_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("DlcGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("games_dlc__dbt_tmp_dlc_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("BaseGameId", "DlcGameId").HasName("games_dlc__dbt_tmp_pkey");
                        j.ToTable("games_dlc");
                        j.IndexerProperty<long>("BaseGameId").HasColumnName("base_game_id");
                        j.IndexerProperty<long>("DlcGameId").HasColumnName("dlc_game_id");
                    });

            entity.HasMany(d => d.DlcGames).WithMany(p => p.BaseGames)
                .UsingEntity<Dictionary<string, object>>(
                    "GamesDlc",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("DlcGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("games_dlc__dbt_tmp_dlc_game_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("BaseGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("games_dlc__dbt_tmp_base_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("BaseGameId", "DlcGameId").HasName("games_dlc__dbt_tmp_pkey");
                        j.ToTable("games_dlc");
                        j.IndexerProperty<long>("BaseGameId").HasColumnName("base_game_id");
                        j.IndexerProperty<long>("DlcGameId").HasColumnName("dlc_game_id");
                    });

            entity.HasMany(d => d.Franchises).WithMany(p => p.Games1)
                .UsingEntity<Dictionary<string, object>>(
                    "GameFranchise",
                    r => r.HasOne<Franchise>().WithMany()
                        .HasForeignKey("FranchiseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_franchise__dbt_tmp_franchise_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_franchise__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "FranchiseId").HasName("game_franchise__dbt_tmp_pkey");
                        j.ToTable("game_franchise");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("FranchiseId").HasColumnName("franchise_id");
                    });

            entity.HasMany(d => d.GameModes).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameGameMode",
                    r => r.HasOne<GameMode>().WithMany()
                        .HasForeignKey("GameModeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_game_mode__dbt_tmp_game_mode_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_game_mode__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "GameModeId").HasName("game_game_mode__dbt_tmp_pkey");
                        j.ToTable("game_game_mode");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("GameModeId").HasColumnName("game_mode_id");
                    });

            entity.HasMany(d => d.Games).WithMany(p => p.SimilarGames)
                .UsingEntity<Dictionary<string, object>>(
                    "GameSimilarGame",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_game_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("SimilarGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_similar_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "SimilarGameId").HasName("game_similar_game__dbt_tmp_pkey");
                        j.ToTable("game_similar_game", tb => tb.HasComment("Join table linking games to their similar games."));
                        j.IndexerProperty<long>("GameId")
                            .HasComment("The game that has a similar game relationship.")
                            .HasColumnName("game_id");
                        j.IndexerProperty<long>("SimilarGameId")
                            .HasComment("The similar game referenced by the relationship.")
                            .HasColumnName("similar_game_id");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_genre__dbt_tmp_genre_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_genre__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "GenreId").HasName("game_genre__dbt_tmp_pkey");
                        j.ToTable("game_genre");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("GenreId").HasColumnName("genre_id");
                    });

            entity.HasMany(d => d.Keywords).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameKeyword",
                    r => r.HasOne<Keyword>().WithMany()
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_keyword__dbt_tmp_keyword_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_keyword__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "KeywordId").HasName("game_keyword__dbt_tmp_pkey");
                        j.ToTable("game_keyword");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("KeywordId").HasColumnName("keyword_id");
                    });

            entity.HasMany(d => d.MultiplayerModes).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameMultiplayerMode",
                    r => r.HasOne<MultiplayerMode>().WithMany()
                        .HasForeignKey("MultiplayerModeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_multiplayer_mode__dbt_tmp_multiplayer_mode_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_multiplayer_mode__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "MultiplayerModeId").HasName("game_multiplayer_mode__dbt_tmp_pkey");
                        j.ToTable("game_multiplayer_mode");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("MultiplayerModeId").HasColumnName("multiplayer_mode_id");
                    });

            entity.HasMany(d => d.Platforms).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GamePlatform",
                    r => r.HasOne<Platform>().WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_platform__dbt_tmp_platform_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_platform__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "PlatformId").HasName("game_platform__dbt_tmp_pkey");
                        j.ToTable("game_platform");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("PlatformId").HasColumnName("platform_id");
                    });

            entity.HasMany(d => d.PlayerPerspectives).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GamePlayerPerspective",
                    r => r.HasOne<PlayerPerspective>().WithMany()
                        .HasForeignKey("PlayerPerspectiveId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_player_perspective__dbt_tmp_player_perspective_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_player_perspective__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "PlayerPerspectiveId").HasName("game_player_perspective__dbt_tmp_pkey");
                        j.ToTable("game_player_perspective");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("PlayerPerspectiveId").HasColumnName("player_perspective_id");
                    });

            entity.HasMany(d => d.SimilarGames).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameSimilarGame",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("SimilarGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_similar_game_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "SimilarGameId").HasName("game_similar_game__dbt_tmp_pkey");
                        j.ToTable("game_similar_game", tb => tb.HasComment("Join table linking games to their similar games."));
                        j.IndexerProperty<long>("GameId")
                            .HasComment("The game that has a similar game relationship.")
                            .HasColumnName("game_id");
                        j.IndexerProperty<long>("SimilarGameId")
                            .HasComment("The similar game referenced by the relationship.")
                            .HasColumnName("similar_game_id");
                    });

            entity.HasMany(d => d.Themes).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameTheme",
                    r => r.HasOne<Theme>().WithMany()
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_theme__dbt_tmp_theme_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_theme__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "ThemeId").HasName("game_theme__dbt_tmp_pkey");
                        j.ToTable("game_theme");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("ThemeId").HasColumnName("theme_id");
                    });
        });

        modelBuilder.Entity<GameEngine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_engines__dbt_tmp_pkey");

            entity.ToTable("game_engines");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.LogoNavigation).WithMany(p => p.GameEngines)
                .HasForeignKey(d => d.Logo)
                .HasConstraintName("game_engines__dbt_tmp_logo_fkey");

            entity.HasMany(d => d.Companies).WithMany(p => p.GameEngines)
                .UsingEntity<Dictionary<string, object>>(
                    "GameEngineCompany",
                    r => r.HasOne<Company>().WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_engine_company__dbt_tmp_company_id_fkey"),
                    l => l.HasOne<GameEngine>().WithMany()
                        .HasForeignKey("GameEngineId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_engine_company__dbt_tmp_game_engine_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameEngineId", "CompanyId").HasName("game_engine_company__dbt_tmp_pkey");
                        j.ToTable("game_engine_company");
                        j.IndexerProperty<long>("GameEngineId").HasColumnName("game_engine_id");
                        j.IndexerProperty<long>("CompanyId").HasColumnName("company_id");
                    });

            entity.HasMany(d => d.Platforms).WithMany(p => p.GameEngines)
                .UsingEntity<Dictionary<string, object>>(
                    "GameEnginePlatform",
                    r => r.HasOne<Platform>().WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_engine_platforms__dbt_tmp_platform_id_fkey"),
                    l => l.HasOne<GameEngine>().WithMany()
                        .HasForeignKey("GameEngineId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_engine_platforms__dbt_tmp_game_engine_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameEngineId", "PlatformId").HasName("game_engine_platforms__dbt_tmp_pkey");
                        j.ToTable("game_engine_platforms");
                        j.IndexerProperty<long>("GameEngineId").HasColumnName("game_engine_id");
                        j.IndexerProperty<long>("PlatformId").HasColumnName("platform_id");
                    });
        });

        modelBuilder.Entity<GameEngineLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_engine_logo__dbt_tmp_pkey");

            entity.ToTable("game_engine_logo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.GameEngineId).HasColumnName("game_engine_id");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<GameGameEngine>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("game_game_engine", tb => tb.HasComment("Join table linking games to game engines in a relational format."));

            entity.Property(e => e.GameEngineId)
                .HasComment("The game engine identifier.")
                .HasColumnName("game_engine_id");
            entity.Property(e => e.GameId)
                .HasComment("The game identifier.")
                .HasColumnName("game_id");
        });

        modelBuilder.Entity<GameLocalization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_localizations__dbt_tmp_pkey");

            entity.ToTable("game_localizations");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Cover).HasColumnName("cover");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.CoverNavigation).WithMany(p => p.GameLocalizations)
                .HasForeignKey(d => d.Cover)
                .HasConstraintName("game_localizations__dbt_tmp_cover_fkey");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.GameLocalizations)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("game_localizations__dbt_tmp_game_fkey");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.GameLocalizations)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("game_localizations__dbt_tmp_region_fkey");
        });

        modelBuilder.Entity<GameMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_modes__dbt_tmp_pkey");

            entity.ToTable("game_modes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<GamePopularityLatest>(entity =>
        {
            entity.HasKey(e => new { e.GameId, e.PopularityType, e.ExternalPopularitySource }).HasName("game_popularity_latest__dbt_tmp_pkey1");

            entity.ToTable("game_popularity_latest", tb => tb.HasComment("Latest popularity snapshot per game, popularity type, and external source."));

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PopularityType).HasColumnName("popularity_type");
            entity.Property(e => e.ExternalPopularitySource).HasColumnName("external_popularity_source");
            entity.Property(e => e.CalculatedAt).HasColumnName("calculated_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Id)
                .HasComment("Source popularity primitive id selected as latest for the key.")
                .HasColumnName("id");
            entity.Property(e => e.SnapshotDate).HasColumnName("snapshot_date");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtTz).HasColumnName("updated_at_tz");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.ExternalPopularitySourceNavigation).WithMany(p => p.GamePopularityLatests)
                .HasForeignKey(d => d.ExternalPopularitySource)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_popularity_latest__dbt_tmp_external_popularity_source_fkey");

            entity.HasOne(d => d.Game).WithMany(p => p.GamePopularityLatests)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_popularity_latest__dbt_tmp_game_id_fkey");

            entity.HasOne(d => d.PopularityTypeNavigation).WithMany(p => p.GamePopularityLatests)
                .HasForeignKey(d => d.PopularityType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_popularity_latest__dbt_tmp_popularity_type_fkey");
        });

        modelBuilder.Entity<GameReleaseFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_release_formats__dbt_tmp_pkey");

            entity.ToTable("game_release_formats");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Format)
                .HasColumnType("character varying")
                .HasColumnName("format");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<GameStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_statuses__dbt_tmp_pkey");

            entity.ToTable("game_statuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<GameType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_types__dbt_tmp_pkey");

            entity.ToTable("game_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<GamesSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("games_search", tb => tb.HasComment("Search-ready games dataset with aggregated metrics for Typesense indexing"));

            entity.Property(e => e.AggregatedRating).HasColumnName("aggregated_rating");
            entity.Property(e => e.AggregatedRatingCount).HasColumnName("aggregated_rating_count");
            entity.Property(e => e.CoverUrl)
                .HasColumnType("character varying")
                .HasColumnName("cover_url");
            entity.Property(e => e.Developers).HasColumnName("developers");
            entity.Property(e => e.FirstReleaseDate).HasColumnName("first_release_date");
            entity.Property(e => e.GameEngines).HasColumnName("game_engines");
            entity.Property(e => e.GameModes).HasColumnName("game_modes");
            entity.Property(e => e.GameStatus)
                .HasColumnType("character varying")
                .HasColumnName("game_status");
            entity.Property(e => e.GameType)
                .HasColumnType("character varying")
                .HasColumnName("game_type");
            entity.Property(e => e.Genres).HasColumnName("genres");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IgdbPlayed).HasColumnName("igdb_played");
            entity.Property(e => e.IgdbPlaying).HasColumnName("igdb_playing");
            entity.Property(e => e.IgdbVisits).HasColumnName("igdb_visits");
            entity.Property(e => e.IgdbWantToPlay).HasColumnName("igdb_want_to_play");
            entity.Property(e => e.MultiplayerModes).HasColumnName("multiplayer_modes");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Platforms).HasColumnName("platforms");
            entity.Property(e => e.PlayerPerspectives).HasColumnName("player_perspectives");
            entity.Property(e => e.Publishers).HasColumnName("publishers");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RatingCount).HasColumnName("rating_count");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Steam24hrPeakPlayers).HasColumnName("steam_24hr_peak_players");
            entity.Property(e => e.SteamGlobalTopSellers).HasColumnName("steam_global_top_sellers");
            entity.Property(e => e.SteamMostWishlistedUpcoming).HasColumnName("steam_most_wishlisted_upcoming");
            entity.Property(e => e.SteamNegativeReviews).HasColumnName("steam_negative_reviews");
            entity.Property(e => e.SteamPositiveReviews).HasColumnName("steam_positive_reviews");
            entity.Property(e => e.SteamTotalReviews).HasColumnName("steam_total_reviews");
            entity.Property(e => e.Storyline)
                .HasColumnType("character varying")
                .HasColumnName("storyline");
            entity.Property(e => e.Summary)
                .HasColumnType("character varying")
                .HasColumnName("summary");
            entity.Property(e => e.Themes).HasColumnName("themes");
            entity.Property(e => e.TotalRating).HasColumnName("total_rating");
            entity.Property(e => e.TotalRatingCount).HasColumnName("total_rating_count");
            entity.Property(e => e.TotalVisits).HasColumnName("total_visits");
            entity.Property(e => e.Twitch24hrHoursWatched).HasColumnName("twitch_24hr_hours_watched");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<GamesSimilarGame>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("games__similar_games", tb => tb.HasComment("Join table representing the similar-games relationship between games."));

            entity.Property(e => e.GameId)
                .HasComment("The source game in the similarity relationship.")
                .HasColumnName("game_id");
            entity.Property(e => e.SimilarGameId)
                .HasComment("The related similar game.")
                .HasColumnName("similar_game_id");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres__dbt_tmp_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<InvolvedCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("involved_companies__dbt_tmp_pkey");

            entity.ToTable("involved_companies");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Company).HasColumnName("company");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Developer).HasColumnName("developer");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Porting).HasColumnName("porting");
            entity.Property(e => e.Publisher).HasColumnName("publisher");
            entity.Property(e => e.Supporting).HasColumnName("supporting");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasMany(d => d.Companies).WithMany(p => p.InvolvedCompanies)
                .UsingEntity<Dictionary<string, object>>(
                    "InvolvedCompanyCompany",
                    r => r.HasOne<Company>().WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("involved_company_company__dbt_tmp_company_id_fkey"),
                    l => l.HasOne<InvolvedCompany>().WithMany()
                        .HasForeignKey("InvolvedCompanyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("involved_company_company__dbt_tmp_involved_company_id_fkey"),
                    j =>
                    {
                        j.HasKey("InvolvedCompanyId", "CompanyId").HasName("involved_company_company__dbt_tmp_pkey");
                        j.ToTable("involved_company_company");
                        j.IndexerProperty<long>("InvolvedCompanyId").HasColumnName("involved_company_id");
                        j.IndexerProperty<long>("CompanyId").HasColumnName("company_id");
                    });
        });

        modelBuilder.Entity<Keyword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("keywords__dbt_tmp_pkey");

            entity.ToTable("keywords");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("languages__dbt_tmp_pkey");

            entity.ToTable("languages");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Locale)
                .HasColumnType("character varying")
                .HasColumnName("locale");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NativeName)
                .HasColumnType("character varying")
                .HasColumnName("native_name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<LanguageSupportType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("language_support_types__dbt_tmp_pkey");

            entity.ToTable("language_support_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<MartCompanySearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("mart_company_search");

            entity.Property(e => e.ChangedCompany)
                .HasColumnType("character varying")
                .HasColumnName("changed_company");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.GamesDevelopedCount).HasColumnName("games_developed_count");
            entity.Property(e => e.GamesPublishedCount).HasColumnName("games_published_count");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogoUrl)
                .HasColumnType("character varying")
                .HasColumnName("logo_url");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.ParentCompany)
                .HasColumnType("character varying")
                .HasColumnName("parent_company");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StartYear).HasColumnName("start_year");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<MartGamesSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("mart_games_search");

            entity.Property(e => e.AggregatedRating).HasColumnName("aggregated_rating");
            entity.Property(e => e.AggregatedRatingCount).HasColumnName("aggregated_rating_count");
            entity.Property(e => e.CoverUrl)
                .HasColumnType("character varying")
                .HasColumnName("cover_url");
            entity.Property(e => e.Developers).HasColumnName("developers");
            entity.Property(e => e.FirstReleaseDate).HasColumnName("first_release_date");
            entity.Property(e => e.GameEngines).HasColumnName("game_engines");
            entity.Property(e => e.GameModes).HasColumnName("game_modes");
            entity.Property(e => e.GameStatus)
                .HasColumnType("character varying")
                .HasColumnName("game_status");
            entity.Property(e => e.GameType)
                .HasColumnType("character varying")
                .HasColumnName("game_type");
            entity.Property(e => e.Genres).HasColumnName("genres");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MultiplayerModes).HasColumnName("multiplayer_modes");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Platforms).HasColumnName("platforms");
            entity.Property(e => e.PlayerPerspectives).HasColumnName("player_perspectives");
            entity.Property(e => e.Publishers).HasColumnName("publishers");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RatingCount).HasColumnName("rating_count");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Storyline)
                .HasColumnType("character varying")
                .HasColumnName("storyline");
            entity.Property(e => e.Summary)
                .HasColumnType("character varying")
                .HasColumnName("summary");
            entity.Property(e => e.Themes).HasColumnName("themes");
            entity.Property(e => e.TotalRating).HasColumnName("total_rating");
            entity.Property(e => e.TotalRatingCount).HasColumnName("total_rating_count");
            entity.Property(e => e.TotalVisits).HasColumnName("total_visits");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<MartGamesSearchPopularityTest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("mart_games_search_popularity_test");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IgdbPlayed).HasColumnName("igdb_played");
            entity.Property(e => e.IgdbPlaying).HasColumnName("igdb_playing");
            entity.Property(e => e.IgdbVisits).HasColumnName("igdb_visits");
            entity.Property(e => e.IgdbWantToPlay).HasColumnName("igdb_want_to_play");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Steam24hrPeakPlayers).HasColumnName("steam_24hr_peak_players");
            entity.Property(e => e.SteamGlobalTopSellers).HasColumnName("steam_global_top_sellers");
            entity.Property(e => e.SteamMostWishlistedUpcoming).HasColumnName("steam_most_wishlisted_upcoming");
            entity.Property(e => e.SteamNegativeReviews).HasColumnName("steam_negative_reviews");
            entity.Property(e => e.SteamPositiveReviews).HasColumnName("steam_positive_reviews");
            entity.Property(e => e.SteamTotalReviews).HasColumnName("steam_total_reviews");
            entity.Property(e => e.Twitch24hrHoursWatched).HasColumnName("twitch_24hr_hours_watched");
        });

        modelBuilder.Entity<MultiplayerMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("multiplayer_modes__dbt_tmp_pkey");

            entity.ToTable("multiplayer_modes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Campaigncoop).HasColumnName("campaigncoop");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Dropin).HasColumnName("dropin");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Lancoop).HasColumnName("lancoop");
            entity.Property(e => e.Offlinecoop).HasColumnName("offlinecoop");
            entity.Property(e => e.Offlinecoopmax).HasColumnName("offlinecoopmax");
            entity.Property(e => e.Offlinemax).HasColumnName("offlinemax");
            entity.Property(e => e.Onlinecoop).HasColumnName("onlinecoop");
            entity.Property(e => e.Onlinecoopmax).HasColumnName("onlinecoopmax");
            entity.Property(e => e.Onlinemax).HasColumnName("onlinemax");
            entity.Property(e => e.Platform).HasColumnName("platform");
            entity.Property(e => e.Splitscreen).HasColumnName("splitscreen");
        });

        modelBuilder.Entity<NetworkType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("network_types__dbt_tmp_pkey");

            entity.ToTable("network_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.EventNetworks)
                .HasColumnType("jsonb")
                .HasColumnName("event_networks");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platforms__dbt_tmp_pkey");

            entity.ToTable("platforms");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Abbreviation)
                .HasColumnType("character varying")
                .HasColumnName("abbreviation");
            entity.Property(e => e.AlternativeName)
                .HasColumnType("character varying")
                .HasColumnName("alternative_name");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Generation).HasColumnName("generation");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PlatformFamily).HasColumnName("platform_family");
            entity.Property(e => e.PlatformLogo).HasColumnName("platform_logo");
            entity.Property(e => e.PlatformType).HasColumnName("platform_type");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.Summary)
                .HasColumnType("character varying")
                .HasColumnName("summary");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.PlatformFamilyNavigation).WithMany(p => p.Platforms)
                .HasForeignKey(d => d.PlatformFamily)
                .HasConstraintName("platforms__dbt_tmp_platform_family_fkey");

            entity.HasOne(d => d.PlatformLogoNavigation).WithMany(p => p.Platforms)
                .HasForeignKey(d => d.PlatformLogo)
                .HasConstraintName("platforms__dbt_tmp_platform_logo_fkey");

            entity.HasOne(d => d.PlatformTypeNavigation).WithMany(p => p.Platforms)
                .HasForeignKey(d => d.PlatformType)
                .HasConstraintName("platforms__dbt_tmp_platform_type_fkey");
        });

        modelBuilder.Entity<PlatformFamily>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_family__dbt_tmp_pkey");

            entity.ToTable("platform_family");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
        });

        modelBuilder.Entity<PlatformLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_logo__dbt_tmp_pkey");

            entity.ToTable("platform_logo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.PlatformId).HasColumnName("platform_id");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<PlatformType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_types__dbt_tmp_pkey");

            entity.ToTable("platform_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<PlatformVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_versions__dbt_tmp_pkey");

            entity.ToTable("platform_versions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Connectivity)
                .HasColumnType("character varying")
                .HasColumnName("connectivity");
            entity.Property(e => e.Cpu)
                .HasColumnType("character varying")
                .HasColumnName("cpu");
            entity.Property(e => e.Graphics)
                .HasColumnType("character varying")
                .HasColumnName("graphics");
            entity.Property(e => e.MainManufacturer).HasColumnName("main_manufacturer");
            entity.Property(e => e.Media)
                .HasColumnType("character varying")
                .HasColumnName("media");
            entity.Property(e => e.Memory)
                .HasColumnType("character varying")
                .HasColumnName("memory");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Os)
                .HasColumnType("character varying")
                .HasColumnName("os");
            entity.Property(e => e.Output)
                .HasColumnType("character varying")
                .HasColumnName("output");
            entity.Property(e => e.PlatformLogo).HasColumnName("platform_logo");
            entity.Property(e => e.Resolutions)
                .HasColumnType("character varying")
                .HasColumnName("resolutions");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.Sound)
                .HasColumnType("character varying")
                .HasColumnName("sound");
            entity.Property(e => e.Storage)
                .HasColumnType("character varying")
                .HasColumnName("storage");
            entity.Property(e => e.Summary)
                .HasColumnType("character varying")
                .HasColumnName("summary");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.MainManufacturerNavigation).WithMany(p => p.PlatformVersions)
                .HasForeignKey(d => d.MainManufacturer)
                .HasConstraintName("platform_versions__dbt_tmp_main_manufacturer_fkey");

            entity.HasOne(d => d.PlatformLogoNavigation).WithMany(p => p.PlatformVersions)
                .HasForeignKey(d => d.PlatformLogo)
                .HasConstraintName("platform_versions__dbt_tmp_platform_logo_fkey");
        });

        modelBuilder.Entity<PlatformVersionCompany1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_version_companies__dbt_tmp_pkey");

            entity.ToTable("platform_version_companies");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.Company).HasColumnName("company");
            entity.Property(e => e.Developer).HasColumnName("developer");
            entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");

            entity.HasOne(d => d.CompanyNavigation).WithMany(p => p.PlatformVersionCompany1s)
                .HasForeignKey(d => d.Company)
                .HasConstraintName("platform_version_companies__dbt_tmp_company_fkey");
        });

        modelBuilder.Entity<PlatformWebsite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_websites__dbt_tmp_pkey");

            entity.ToTable("platform_websites");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Trusted).HasColumnName("trusted");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<PlayerPerspective>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("player_perspectives__dbt_tmp_pkey");

            entity.ToTable("player_perspectives");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<PopularityPrimitive>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("popularity_primitives__dbt_tmp_pkey");

            entity.ToTable("popularity_primitives");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalculatedAt).HasColumnName("calculated_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExternalPopularitySource).HasColumnName("external_popularity_source");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PopularityType).HasColumnName("popularity_type");
            entity.Property(e => e.SnapshotDate).HasColumnName("snapshot_date");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtTz).HasColumnName("updated_at_tz");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.ExternalPopularitySourceNavigation).WithMany(p => p.PopularityPrimitives)
                .HasForeignKey(d => d.ExternalPopularitySource)
                .HasConstraintName("popularity_primitives__dbt_tmp_external_popularity_source_fkey");

            entity.HasOne(d => d.Game).WithMany(p => p.PopularityPrimitives)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("popularity_primitives__dbt_tmp_game_id_fkey");

            entity.HasOne(d => d.PopularityTypeNavigation).WithMany(p => p.PopularityPrimitives)
                .HasForeignKey(d => d.PopularityType)
                .HasConstraintName("popularity_primitives__dbt_tmp_popularity_type_fkey");
        });

        modelBuilder.Entity<PopularityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("popularity_types__dbt_tmp_pkey");

            entity.ToTable("popularity_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExternalPopularitySource).HasColumnName("external_popularity_source");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PopularitySource).HasColumnName("popularity_source");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("regions__dbt_tmp_pkey");

            entity.ToTable("regions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category)
                .HasColumnType("character varying")
                .HasColumnName("category");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Identifier)
                .HasColumnType("character varying")
                .HasColumnName("identifier");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<ReleaseDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("release_dates__dbt_tmp_pkey");

            entity.ToTable("release_dates");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DateFormat).HasColumnName("date_format");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Human)
                .HasColumnType("character varying")
                .HasColumnName("human");
            entity.Property(e => e.M).HasColumnName("m");
            entity.Property(e => e.Platform).HasColumnName("platform");
            entity.Property(e => e.ReleaseRegion).HasColumnName("release_region");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Y).HasColumnName("y");

            entity.HasOne(d => d.DateFormatNavigation).WithMany(p => p.ReleaseDates)
                .HasForeignKey(d => d.DateFormat)
                .HasConstraintName("release_dates__dbt_tmp_date_format_fkey");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.ReleaseDates)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("release_dates__dbt_tmp_game_fkey");

            entity.HasOne(d => d.PlatformNavigation).WithMany(p => p.ReleaseDates)
                .HasForeignKey(d => d.Platform)
                .HasConstraintName("release_dates__dbt_tmp_platform_fkey");

            entity.HasOne(d => d.ReleaseRegionNavigation).WithMany(p => p.ReleaseDates)
                .HasForeignKey(d => d.ReleaseRegion)
                .HasConstraintName("release_dates__dbt_tmp_release_region_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.ReleaseDates)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("release_dates__dbt_tmp_status_fkey");
        });

        modelBuilder.Entity<ReleaseDateRegion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("release_date_regions__dbt_tmp_pkey");

            entity.ToTable("release_date_regions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Region)
                .HasColumnType("character varying")
                .HasColumnName("region");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<ReleaseDateStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("release_date_statuses__dbt_tmp_pkey");

            entity.ToTable("release_date_statuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Screenshot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("screenshots__dbt_tmp_pkey");

            entity.ToTable("screenshots");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AlphaChannel).HasColumnName("alpha_channel");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.ImageId)
                .HasColumnType("character varying")
                .HasColumnName("image_id");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Width).HasColumnName("width");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.Screenshots)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("screenshots__dbt_tmp_game_fkey");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("themes__dbt_tmp_pkey");

            entity.ToTable("themes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("videos__dbt_tmp_pkey");

            entity.ToTable("videos");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.VideoId)
                .HasColumnType("character varying")
                .HasColumnName("video_id");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.Videos)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("videos__dbt_tmp_game_fkey");
        });

        modelBuilder.Entity<Website>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("websites__dbt_tmp_pkey");

            entity.ToTable("websites");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.Trusted).HasColumnName("trusted");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.Websites)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("websites__dbt_tmp_game_fkey");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Websites)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("websites__dbt_tmp_type_fkey");
        });

        modelBuilder.Entity<WebsiteType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("website_types__dbt_tmp_pkey");

            entity.ToTable("website_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
