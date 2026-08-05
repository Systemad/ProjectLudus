using System;
using System.Collections.Generic;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public partial class AppDbContext : DbContext
{
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

    public virtual DbSet<BridgeCompanyTypeHistory> BridgeCompanyTypeHistories { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterGender> CharacterGenders { get; set; }

    public virtual DbSet<CharacterMugShot> CharacterMugShots { get; set; }

    public virtual DbSet<CharacterSpecy> CharacterSpecies { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<CollectionMembership> CollectionMemberships { get; set; }

    public virtual DbSet<CollectionMembershipType> CollectionMembershipTypes { get; set; }

    public virtual DbSet<CollectionRelation> CollectionRelations { get; set; }

    public virtual DbSet<CollectionRelationType> CollectionRelationTypes { get; set; }

    public virtual DbSet<CollectionType> CollectionTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyLogo> CompanyLogos { get; set; }

    public virtual DbSet<CompanySearch> CompanySearches { get; set; }

    public virtual DbSet<CompanySize> CompanySizes { get; set; }

    public virtual DbSet<CompanyStatus> CompanyStatuses { get; set; }

    public virtual DbSet<CompanyType> CompanyTypes { get; set; }

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

    public virtual DbSet<GameExpandedGame> GameExpandedGames { get; set; }

    public virtual DbSet<GameExpansion> GameExpansions { get; set; }

    public virtual DbSet<GameFork> GameForks { get; set; }

    public virtual DbSet<GameLocalization> GameLocalizations { get; set; }

    public virtual DbSet<GameMode> GameModes { get; set; }

    public virtual DbSet<GamePort> GamePorts { get; set; }

    public virtual DbSet<GameReleaseFormat> GameReleaseFormats { get; set; }

    public virtual DbSet<GameRemake> GameRemakes { get; set; }

    public virtual DbSet<GameRemaster> GameRemasters { get; set; }

    public virtual DbSet<GameStandaloneExpansion> GameStandaloneExpansions { get; set; }

    public virtual DbSet<GameStatus> GameStatuses { get; set; }

    public virtual DbSet<GameTimeToBeat> GameTimeToBeats { get; set; }

    public virtual DbSet<GameType> GameTypes { get; set; }

    public virtual DbSet<GamesDlc> GamesDlcs { get; set; }

    public virtual DbSet<GamesSearch> GamesSearches { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<InvolvedCompany> InvolvedCompanies { get; set; }

    public virtual DbSet<Keyword> Keywords { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LanguageSupport> LanguageSupports { get; set; }

    public virtual DbSet<LanguageSupportType> LanguageSupportTypes { get; set; }

    public virtual DbSet<MultiplayerMode> MultiplayerModes { get; set; }

    public virtual DbSet<NetworkType> NetworkTypes { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<PlatformFamily> PlatformFamilies { get; set; }

    public virtual DbSet<PlatformLogo> PlatformLogos { get; set; }

    public virtual DbSet<PlatformType> PlatformTypes { get; set; }

    public virtual DbSet<PlatformVersion> PlatformVersions { get; set; }

    public virtual DbSet<PlatformVersionCompany> PlatformVersionCompanies { get; set; }

    public virtual DbSet<PlatformVersionReleaseDate> PlatformVersionReleaseDates { get; set; }

    public virtual DbSet<PlatformWebsite> PlatformWebsites { get; set; }

    public virtual DbSet<PlayerPerspective> PlayerPerspectives { get; set; }

    public virtual DbSet<PopularityPrimitive> PopularityPrimitives { get; set; }

    public virtual DbSet<PopularityType> PopularityTypes { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<ReleaseDate> ReleaseDates { get; set; }

    public virtual DbSet<ReleaseDateRegion> ReleaseDateRegions { get; set; }

    public virtual DbSet<ReleaseDateStatus> ReleaseDateStatuses { get; set; }

    public virtual DbSet<Screenshot> Screenshots { get; set; }

    public virtual DbSet<SteamDetail> SteamDetails { get; set; }

    public virtual DbSet<SteamLatestPlayerCount> SteamLatestPlayerCounts { get; set; }

    public virtual DbSet<SteamLatestPricing> SteamLatestPricings { get; set; }

    public virtual DbSet<SteamPlayerStatsDaily> SteamPlayerStatsDailies { get; set; }

    public virtual DbSet<SteamPlayerStatsHourly> SteamPlayerStatsHourlies { get; set; }

    public virtual DbSet<SteamPricingDaily> SteamPricingDailies { get; set; }

    public virtual DbSet<SteamPricingHourly> SteamPricingHourlies { get; set; }

    public virtual DbSet<SteamReview> SteamReviews { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<TrackedGame> TrackedGames { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<Website> Websites { get; set; }

    public virtual DbSet<WebsiteType> WebsiteTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("timescaledb");

        modelBuilder.Entity<AgeRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("age_ratings__dbt_tmp_pkey1");

            entity.ToTable("age_ratings", "igdb");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Organization).HasColumnName("organization");
            entity.Property(e => e.RatingCategory).HasColumnName("rating_category");
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

            entity.ToTable("age_rating_categories", "igdb");

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

            entity.ToTable("age_rating_content_description_types", "igdb");

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

            entity.ToTable("age_rating_content_descriptions_v2", "igdb");

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

            entity.ToTable("age_rating_organizations", "igdb");

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
            entity.HasKey(e => e.Id).HasName("alternative_names__dbt_tmp_pkey1");

            entity.ToTable("alternative_names", "igdb");

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
            entity.HasKey(e => e.Id).HasName("artworks__dbt_tmp_pkey1");

            entity.ToTable("artworks", "igdb");

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

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.Artworks)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("artworks__dbt_tmp_game_fkey");
        });

        modelBuilder.Entity<ArtworkType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artwork_types__dbt_tmp_pkey");

            entity.ToTable("artwork_types", "igdb");

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

        modelBuilder.Entity<BridgeCompanyTypeHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("bridge_company_type_history", "igdb");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CompanyTypeId).HasColumnName("company_type_id");

            entity.HasOne(d => d.Company).WithMany()
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bridge_company_type_history__dbt_tmp_company_id_fkey");

            entity.HasOne(d => d.CompanyType).WithMany()
                .HasForeignKey(d => d.CompanyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bridge_company_type_history__dbt_tmp_company_type_id_fkey");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("characters__dbt_tmp_pkey");

            entity.ToTable("characters", "igdb");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CharacterGender).HasColumnName("character_gender");
            entity.Property(e => e.CharacterSpecies).HasColumnName("character_species");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
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

            entity.ToTable("character_genders", "igdb");

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
            entity.HasKey(e => e.Id).HasName("character_mug_shots__dbt_tmp_pkey1");

            entity.ToTable("character_mug_shots", "igdb");

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

            entity.ToTable("character_species", "igdb");

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

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("collections__dbt_tmp_pkey");

            entity.ToTable("collections", "igdb", tb => tb.HasComment("Game collections (series, bundles, etc.)."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasComment("Collection name.")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasComment("Collection slug.")
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.Type)
                .HasComment("FK to mart_collection_types.id.")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasComment("Collection URL.")
                .HasColumnType("character varying")
                .HasColumnName("url");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Collections)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("collections__dbt_tmp_type_fkey");
        });

        modelBuilder.Entity<CollectionMembership>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("collection_memberships__dbt_tmp_pkey1");

            entity.ToTable("collection_memberships", "igdb", tb => tb.HasComment("Links games to collections with a membership type."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Collection)
                .HasComment("FK to mart_collections.id.")
                .HasColumnName("collection");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Game)
                .HasComment("FK to mart_games.id.")
                .HasColumnName("game");
            entity.Property(e => e.Type)
                .HasComment("FK to mart_collection_membership_types.id.")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.CollectionNavigation).WithMany(p => p.CollectionMemberships)
                .HasForeignKey(d => d.Collection)
                .HasConstraintName("collection_memberships__dbt_tmp_collection_fkey");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.CollectionMemberships)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("collection_memberships__dbt_tmp_game_fkey");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.CollectionMemberships)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("collection_memberships__dbt_tmp_type_fkey");
        });

        modelBuilder.Entity<CollectionMembershipType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("collection_membership_types__dbt_tmp_pkey");

            entity.ToTable("collection_membership_types", "igdb", tb => tb.HasComment("Lookup table for collection membership types."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.AllowedCollectionType)
                .HasComment("FK to mart_collection_types.id.")
                .HasColumnName("allowed_collection_type");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasComment("Type description.")
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasComment("Type name.")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.AllowedCollectionTypeNavigation).WithMany(p => p.CollectionMembershipTypes)
                .HasForeignKey(d => d.AllowedCollectionType)
                .HasConstraintName("collection_membership_types__dbt_t_allowed_collection_type_fkey");
        });

        modelBuilder.Entity<CollectionRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("collection_relations__dbt_tmp_pkey");

            entity.ToTable("collection_relations", "igdb", tb => tb.HasComment("Hierarchy links between collections (child-parent)."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.ChildCollection)
                .HasComment("FK to mart_collections.id.")
                .HasColumnName("child_collection");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ParentCollection)
                .HasComment("FK to mart_collections.id.")
                .HasColumnName("parent_collection");
            entity.Property(e => e.Type)
                .HasComment("FK to mart_collection_relation_types.id.")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.ChildCollectionNavigation).WithMany(p => p.CollectionRelationChildCollectionNavigations)
                .HasForeignKey(d => d.ChildCollection)
                .HasConstraintName("collection_relations__dbt_tmp_child_collection_fkey");

            entity.HasOne(d => d.ParentCollectionNavigation).WithMany(p => p.CollectionRelationParentCollectionNavigations)
                .HasForeignKey(d => d.ParentCollection)
                .HasConstraintName("collection_relations__dbt_tmp_parent_collection_fkey");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.CollectionRelations)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("collection_relations__dbt_tmp_type_fkey");
        });

        modelBuilder.Entity<CollectionRelationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("collection_relation_types__dbt_tmp_pkey");

            entity.ToTable("collection_relation_types", "igdb", tb => tb.HasComment("Lookup table for collection relation types."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.AllowedChildType)
                .HasComment("FK to mart_collection_types.id — allowed type for child.")
                .HasColumnName("allowed_child_type");
            entity.Property(e => e.AllowedParentType)
                .HasComment("FK to mart_collection_types.id — allowed type for parent.")
                .HasColumnName("allowed_parent_type");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasComment("Type description.")
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasComment("Type name.")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.AllowedChildTypeNavigation).WithMany(p => p.CollectionRelationTypeAllowedChildTypeNavigations)
                .HasForeignKey(d => d.AllowedChildType)
                .HasConstraintName("collection_relation_types__dbt_tmp_allowed_child_type_fkey");

            entity.HasOne(d => d.AllowedParentTypeNavigation).WithMany(p => p.CollectionRelationTypeAllowedParentTypeNavigations)
                .HasForeignKey(d => d.AllowedParentType)
                .HasConstraintName("collection_relation_types__dbt_tmp_allowed_parent_type_fkey");
        });

        modelBuilder.Entity<CollectionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("collection_types__dbt_tmp_pkey1");

            entity.ToTable("collection_types", "igdb", tb => tb.HasComment("collection_types lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companies__dbt_tmp_pkey");

            entity.ToTable("companies", "igdb");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ChangeDate).HasColumnName("change_date");
            entity.Property(e => e.ChangeDateFormat).HasColumnName("change_date_format");
            entity.Property(e => e.ChangedCompanyId).HasColumnName("changed_company_id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CompanySize).HasColumnName("company_size");
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

            entity.HasOne(d => d.CompanySizeNavigation).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CompanySize)
                .HasConstraintName("companies__dbt_tmp_company_size_fkey");

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
        });

        modelBuilder.Entity<CompanyLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_logos__dbt_tmp_pkey1");

            entity.ToTable("company_logos", "igdb");

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
            entity.HasKey(e => e.Id).HasName("company_search__dbt_tmp_pkey1");

            entity.ToTable("company_search", "igdb", tb => tb.HasComment("Search-ready companies dataset for Typesense indexing"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ChangedCompany)
                .HasColumnType("character varying")
                .HasColumnName("changed_company");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.GamesDevelopedCount).HasColumnName("games_developed_count");
            entity.Property(e => e.GamesPublishedCount).HasColumnName("games_published_count");
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

        modelBuilder.Entity<CompanySize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_sizes__dbt_tmp_pkey1");

            entity.ToTable("company_sizes", "igdb", tb => tb.HasComment("company_sizes lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<CompanyStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_statuses__dbt_tmp_pkey1");

            entity.ToTable("company_statuses", "igdb", tb => tb.HasComment("company_statuses lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<CompanyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_types__dbt_tmp_pkey1");

            entity.ToTable("company_types", "igdb", tb => tb.HasComment("company_types lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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
            entity.HasKey(e => e.Id).HasName("company_websites__dbt_tmp_pkey1");

            entity.ToTable("company_websites", "igdb");

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
            entity.HasKey(e => e.Id).HasName("covers__dbt_tmp_pkey1");

            entity.ToTable("covers", "igdb");

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
            entity.HasKey(e => e.Id).HasName("date_formats__dbt_tmp_pkey1");

            entity.ToTable("date_formats", "igdb", tb => tb.HasComment("date_formats lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

            entity.ToTable("events", "igdb");

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
            entity.Property(e => e.EndTimeEpoch).HasColumnName("end_time_epoch");
            entity.Property(e => e.EndTimeUtc).HasColumnName("end_time_utc");
            entity.Property(e => e.EventLogo).HasColumnName("event_logo");
            entity.Property(e => e.LiveStreamUrl)
                .HasColumnType("character varying")
                .HasColumnName("live_stream_url");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.StartTimeEpoch).HasColumnName("start_time_epoch");
            entity.Property(e => e.StartTimeUtc).HasColumnName("start_time_utc");
            entity.Property(e => e.TimeZone)
                .HasColumnType("character varying")
                .HasColumnName("time_zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.EventLogoNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventLogo)
                .HasConstraintName("events__dbt_tmp_event_logo_fkey");

            entity.HasMany(d => d.EventNetworksNavigation).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventNetwork1",
                    r => r.HasOne<EventNetwork>().WithMany()
                        .HasForeignKey("EventNetworkId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_network__dbt_tmp_event_network_id_fkey"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_network__dbt_tmp_event_id_fkey"),
                    j =>
                    {
                        j.HasKey("EventId", "EventNetworkId").HasName("event_network__dbt_tmp_pkey");
                        j.ToTable("event_network", "igdb");
                        j.IndexerProperty<long>("EventId").HasColumnName("event_id");
                        j.IndexerProperty<long>("EventNetworkId").HasColumnName("event_network_id");
                    });

            entity.HasMany(d => d.Games).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventGame",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_game__dbt_tmp_game_id_fkey"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_game__dbt_tmp_event_id_fkey"),
                    j =>
                    {
                        j.HasKey("EventId", "GameId").HasName("event_game__dbt_tmp_pkey");
                        j.ToTable("event_game", "igdb");
                        j.IndexerProperty<long>("EventId").HasColumnName("event_id");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                    });

            entity.HasMany(d => d.Videos).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventVideo",
                    r => r.HasOne<Video>().WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_video__dbt_tmp_video_id_fkey"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_video__dbt_tmp_event_id_fkey"),
                    j =>
                    {
                        j.HasKey("EventId", "VideoId").HasName("event_video__dbt_tmp_pkey");
                        j.ToTable("event_video", "igdb");
                        j.IndexerProperty<long>("EventId").HasColumnName("event_id");
                        j.IndexerProperty<long>("VideoId").HasColumnName("video_id");
                    });
        });

        modelBuilder.Entity<EventLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_logos__dbt_tmp_pkey");

            entity.ToTable("event_logos", "igdb");

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

            entity.ToTable("event_networks", "igdb");

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

            entity.HasOne(d => d.EventNavigation).WithMany(p => p.EventNetworks)
                .HasForeignKey(d => d.Event)
                .HasConstraintName("event_networks__dbt_tmp_event_fkey");

            entity.HasOne(d => d.NetworkTypeNavigation).WithMany(p => p.EventNetworks)
                .HasForeignKey(d => d.NetworkType)
                .HasConstraintName("event_networks__dbt_tmp_network_type_fkey");
        });

        modelBuilder.Entity<ExternalGame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("external_games__dbt_tmp_pkey1");

            entity.ToTable("external_games", "igdb");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExternalGameSource).HasColumnName("external_game_source");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.GameReleaseFormat).HasColumnName("game_release_format");
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

            entity.ToTable("external_game_sources", "igdb");

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

            entity.ToTable("franchises", "igdb");

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

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games__dbt_tmp_pkey1");

            entity.ToTable("games", "igdb");

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
            entity.Property(e => e.FirstReleaseDateEpoch).HasColumnName("first_release_date_epoch");
            entity.Property(e => e.FirstReleaseDateUtc).HasColumnName("first_release_date_utc");
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

            entity.HasOne(d => d.FranchiseNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.Franchise)
                .HasConstraintName("games__dbt_tmp_franchise_fkey");

            entity.HasOne(d => d.GameStatusNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameStatus)
                .HasConstraintName("games__dbt_tmp_game_status_fkey");

            entity.HasOne(d => d.GameTypeNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameType)
                .HasConstraintName("games__dbt_tmp_game_type_fkey");

            entity.HasMany(d => d.Franchises).WithMany(p => p.GamesNavigation)
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
                        j.ToTable("game_franchise", "igdb");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("FranchiseId").HasColumnName("franchise_id");
                    });

            entity.HasMany(d => d.GameEngines).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameGameEngine",
                    r => r.HasOne<GameEngine>().WithMany()
                        .HasForeignKey("GameEngineId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_game_engine__dbt_tmp_game_engine_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_game_engine__dbt_tmp_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("GameId", "GameEngineId").HasName("game_game_engine__dbt_tmp_pkey");
                        j.ToTable("game_game_engine", "igdb", tb => tb.HasComment("Join table linking games to game engines in a relational format."));
                        j.IndexerProperty<long>("GameId")
                            .HasComment("The game identifier.")
                            .HasColumnName("game_id");
                        j.IndexerProperty<long>("GameEngineId")
                            .HasComment("The game engine identifier.")
                            .HasColumnName("game_engine_id");
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
                        j.ToTable("game_game_mode", "igdb");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("GameModeId").HasColumnName("game_mode_id");
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
                        j.ToTable("game_genre", "igdb");
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
                        j.ToTable("game_keyword", "igdb");
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
                        j.ToTable("game_multiplayer_mode", "igdb");
                        j.HasIndex(new[] { "MultiplayerModeId" }, "idx_game_multiplayer_mode_multiplayer_mode_id");
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
                        j.ToTable("game_platform", "igdb");
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
                        j.ToTable("game_player_perspective", "igdb");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("PlayerPerspectiveId").HasColumnName("player_perspective_id");
                    });

            entity.HasMany(d => d.SimilarGames).WithMany(p => p.SimilarSources)
                .UsingEntity<Dictionary<string, object>>(
                    "GameSimilarGame",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("SimilarGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_similar_game_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("SimilarSourceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_similar_source_id_fkey"),
                    j =>
                    {
                        j.HasKey("SimilarSourceId", "SimilarGameId").HasName("game_similar_game__dbt_tmp_pkey1");
                        j.ToTable("game_similar_game", "igdb", tb => tb.HasComment("Join table linking games to their similar games."));
                        j.IndexerProperty<long>("SimilarSourceId").HasColumnName("similar_source_id");
                        j.IndexerProperty<long>("SimilarGameId").HasColumnName("similar_game_id");
                    });

            entity.HasMany(d => d.SimilarSources).WithMany(p => p.SimilarGames)
                .UsingEntity<Dictionary<string, object>>(
                    "GameSimilarGame",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("SimilarSourceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_similar_source_id_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("SimilarGameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("game_similar_game__dbt_tmp_similar_game_id_fkey"),
                    j =>
                    {
                        j.HasKey("SimilarSourceId", "SimilarGameId").HasName("game_similar_game__dbt_tmp_pkey1");
                        j.ToTable("game_similar_game", "igdb", tb => tb.HasComment("Join table linking games to their similar games."));
                        j.IndexerProperty<long>("SimilarSourceId").HasColumnName("similar_source_id");
                        j.IndexerProperty<long>("SimilarGameId").HasColumnName("similar_game_id");
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
                        j.ToTable("game_theme", "igdb");
                        j.IndexerProperty<long>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<long>("ThemeId").HasColumnName("theme_id");
                    });
        });

        modelBuilder.Entity<GameEngine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_engines__dbt_tmp_pkey");

            entity.ToTable("game_engines", "igdb");

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
                        j.ToTable("game_engine_company", "igdb");
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
                        j.ToTable("game_engine_platforms", "igdb");
                        j.IndexerProperty<long>("GameEngineId").HasColumnName("game_engine_id");
                        j.IndexerProperty<long>("PlatformId").HasColumnName("platform_id");
                    });
        });

        modelBuilder.Entity<GameEngineLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_engine_logos__dbt_tmp_pkey1");

            entity.ToTable("game_engine_logos", "igdb");

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

        modelBuilder.Entity<GameExpandedGame>(entity =>
        {
            entity.HasKey(e => e.ExpandedGameId).HasName("game_expanded_game__dbt_tmp_pkey1");

            entity.ToTable("game_expanded_game", "igdb", tb => tb.HasComment("Join table linking games to their expanded games."));

            entity.Property(e => e.ExpandedGameId)
                .ValueGeneratedNever()
                .HasColumnName("expanded_game_id");
            entity.Property(e => e.ExpandedSourceId).HasColumnName("expanded_source_id");

            entity.HasOne(d => d.ExpandedGame).WithOne(p => p.GameExpandedGame)
                .HasForeignKey<GameExpandedGame>(d => d.ExpandedGameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_expanded_game__dbt_tmp_expanded_game_id_fkey");
        });

        modelBuilder.Entity<GameExpansion>(entity =>
        {
            entity.HasKey(e => e.ExpansionId).HasName("game_expansion__dbt_tmp_pkey");

            entity.ToTable("game_expansion", "igdb", tb => tb.HasComment("Join table linking games to their expansions."));

            entity.Property(e => e.ExpansionId)
                .ValueGeneratedNever()
                .HasColumnName("expansion_id");
            entity.Property(e => e.ExpansionSourceId).HasColumnName("expansion_source_id");

            entity.HasOne(d => d.Expansion).WithOne(p => p.GameExpansion)
                .HasForeignKey<GameExpansion>(d => d.ExpansionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_expansion__dbt_tmp_expansion_id_fkey");
        });

        modelBuilder.Entity<GameFork>(entity =>
        {
            entity.HasKey(e => e.ForkId).HasName("game_fork__dbt_tmp_pkey");

            entity.ToTable("game_fork", "igdb", tb => tb.HasComment("Join table linking games to their forks."));

            entity.Property(e => e.ForkId)
                .ValueGeneratedNever()
                .HasColumnName("fork_id");
            entity.Property(e => e.ForkSourceId).HasColumnName("fork_source_id");

            entity.HasOne(d => d.Fork).WithOne(p => p.GameFork)
                .HasForeignKey<GameFork>(d => d.ForkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_fork__dbt_tmp_fork_id_fkey");
        });

        modelBuilder.Entity<GameLocalization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_localizations__dbt_tmp_pkey1");

            entity.ToTable("game_localizations", "igdb");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_localizations__dbt_tmp_game_fkey");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.GameLocalizations)
                .HasForeignKey(d => d.Region)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_localizations__dbt_tmp_region_fkey");
        });

        modelBuilder.Entity<GameMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_modes__dbt_tmp_pkey1");

            entity.ToTable("game_modes", "igdb", tb => tb.HasComment("game_modes lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<GamePort>(entity =>
        {
            entity.HasKey(e => e.PortId).HasName("game_port__dbt_tmp_pkey1");

            entity.ToTable("game_port", "igdb", tb => tb.HasComment("Join table linking games to their ports."));

            entity.Property(e => e.PortId)
                .ValueGeneratedNever()
                .HasColumnName("port_id");
            entity.Property(e => e.PortSourceId).HasColumnName("port_source_id");

            entity.HasOne(d => d.Port).WithOne(p => p.GamePort)
                .HasForeignKey<GamePort>(d => d.PortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_port__dbt_tmp_port_id_fkey");
        });

        modelBuilder.Entity<GameReleaseFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_release_formats__dbt_tmp_pkey1");

            entity.ToTable("game_release_formats", "igdb", tb => tb.HasComment("game_release_formats lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<GameRemake>(entity =>
        {
            entity.HasKey(e => e.RemakeId).HasName("game_remake__dbt_tmp_pkey1");

            entity.ToTable("game_remake", "igdb", tb => tb.HasComment("Join table linking games to their remakes."));

            entity.Property(e => e.RemakeId)
                .ValueGeneratedNever()
                .HasColumnName("remake_id");
            entity.Property(e => e.RemakeSourceId).HasColumnName("remake_source_id");

            entity.HasOne(d => d.Remake).WithOne(p => p.GameRemake)
                .HasForeignKey<GameRemake>(d => d.RemakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_remake__dbt_tmp_remake_id_fkey");
        });

        modelBuilder.Entity<GameRemaster>(entity =>
        {
            entity.HasKey(e => e.RemasterId).HasName("game_remaster__dbt_tmp_pkey");

            entity.ToTable("game_remaster", "igdb", tb => tb.HasComment("Join table linking games to their remasters."));

            entity.Property(e => e.RemasterId)
                .ValueGeneratedNever()
                .HasColumnName("remaster_id");
            entity.Property(e => e.RemasterSourceId).HasColumnName("remaster_source_id");

            entity.HasOne(d => d.Remaster).WithOne(p => p.GameRemaster)
                .HasForeignKey<GameRemaster>(d => d.RemasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_remaster__dbt_tmp_remaster_id_fkey");
        });

        modelBuilder.Entity<GameStandaloneExpansion>(entity =>
        {
            entity.HasKey(e => e.StandaloneExpansionId).HasName("game_standalone_expansion__dbt_tmp_pkey");

            entity.ToTable("game_standalone_expansion", "igdb", tb => tb.HasComment("Join table linking games to their standalone expansions."));

            entity.Property(e => e.StandaloneExpansionId)
                .ValueGeneratedNever()
                .HasColumnName("standalone_expansion_id");
            entity.Property(e => e.StandaloneExpansionSourceId).HasColumnName("standalone_expansion_source_id");

            entity.HasOne(d => d.StandaloneExpansion).WithOne(p => p.GameStandaloneExpansion)
                .HasForeignKey<GameStandaloneExpansion>(d => d.StandaloneExpansionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_standalone_expansion__dbt_tmp_standalone_expansion_id_fkey");
        });

        modelBuilder.Entity<GameStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_statuses__dbt_tmp_pkey1");

            entity.ToTable("game_statuses", "igdb", tb => tb.HasComment("game_statuses lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<GameTimeToBeat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_time_to_beats__dbt_tmp_pkey");

            entity.ToTable("game_time_to_beats", "igdb", tb => tb.HasComment("Time-to-beats metrics for a game — how long to finish (hastily, normally, completely)."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Completely)
                .HasComment("Time to beat (100% completion) in minutes.")
                .HasColumnName("completely");
            entity.Property(e => e.Count)
                .HasComment("Number of submissions.")
                .HasColumnName("count");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.GameId)
                .HasComment("FK to mart_games.id — the game this time-to-beat record belongs to.")
                .HasColumnName("game_id");
            entity.Property(e => e.Hastily)
                .HasComment("Time to beat (rushing) in minutes.")
                .HasColumnName("hastily");
            entity.Property(e => e.Normally)
                .HasComment("Time to beat (average) in minutes.")
                .HasColumnName("normally");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Game).WithMany(p => p.GameTimeToBeats)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("game_time_to_beats__dbt_tmp_game_id_fkey");
        });

        modelBuilder.Entity<GameType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_types__dbt_tmp_pkey1");

            entity.ToTable("game_types", "igdb", tb => tb.HasComment("game_types lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<GamesDlc>(entity =>
        {
            entity.HasKey(e => e.DlcGameId).HasName("games_dlc__dbt_tmp_pkey1");

            entity.ToTable("games_dlc", "igdb", tb => tb.HasComment("Join table linking games to their DLCs."));

            entity.Property(e => e.DlcGameId)
                .ValueGeneratedNever()
                .HasColumnName("dlc_game_id");
            entity.Property(e => e.DlcSourceId).HasColumnName("dlc_source_id");

            entity.HasOne(d => d.DlcGame).WithOne(p => p.GamesDlc)
                .HasForeignKey<GamesDlc>(d => d.DlcGameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("games_dlc__dbt_tmp_dlc_game_id_fkey");
        });

        modelBuilder.Entity<GamesSearch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games_search__dbt_tmp_pkey1");

            entity.ToTable("games_search", "igdb", tb => tb.HasComment("Search-ready games dataset with aggregated metrics for Typesense indexing"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AggregatedRating).HasColumnName("aggregated_rating");
            entity.Property(e => e.AggregatedRatingCount).HasColumnName("aggregated_rating_count");
            entity.Property(e => e.CoverUrl)
                .HasColumnType("character varying")
                .HasColumnName("cover_url");
            entity.Property(e => e.Developers).HasColumnName("developers");
            entity.Property(e => e.FirstReleaseDateEpoch).HasColumnName("first_release_date_epoch");
            entity.Property(e => e.FirstReleaseDateUtc).HasColumnName("first_release_date_utc");
            entity.Property(e => e.GameEngines).HasColumnName("game_engines");
            entity.Property(e => e.GameModes).HasColumnName("game_modes");
            entity.Property(e => e.GameStatus)
                .HasColumnType("character varying")
                .HasColumnName("game_status");
            entity.Property(e => e.GameType)
                .HasColumnType("character varying")
                .HasColumnName("game_type");
            entity.Property(e => e.Genres).HasColumnName("genres");
            entity.Property(e => e.Hypes).HasColumnName("hypes");
            entity.Property(e => e.MultiplayerModes).HasColumnName("multiplayer_modes");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Platforms).HasColumnName("platforms");
            entity.Property(e => e.PlayerPerspectives).HasColumnName("player_perspectives");
            entity.Property(e => e.PopularityScore).HasColumnName("popularity_score");
            entity.Property(e => e.Publishers).HasColumnName("publishers");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RatingCount).HasColumnName("rating_count");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Storyline).HasColumnName("storyline");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Themes).HasColumnName("themes");
            entity.Property(e => e.TotalRating).HasColumnName("total_rating");
            entity.Property(e => e.TotalRatingCount).HasColumnName("total_rating_count");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres__dbt_tmp_pkey1");

            entity.ToTable("genres", "igdb", tb => tb.HasComment("genres lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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
            entity.HasKey(e => e.Id).HasName("involved_companies__dbt_tmp_pkey1");

            entity.ToTable("involved_companies", "igdb", tb => tb.HasComment("Relationship rows that connect games with involved companies and their roles."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("The uniqueness key for the involved-company relationship row.")
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.Company)
                .HasComment("The company involved with the game.")
                .HasColumnName("company");
            entity.Property(e => e.CreatedAt)
                .HasComment("The source record creation timestamp.")
                .HasColumnName("created_at");
            entity.Property(e => e.Developer)
                .HasComment("True when the company is a developer for this game.")
                .HasColumnName("developer");
            entity.Property(e => e.Game)
                .HasComment("The game referenced by this involved-company relationship.")
                .HasColumnName("game");
            entity.Property(e => e.Porting).HasColumnName("porting");
            entity.Property(e => e.Publisher).HasColumnName("publisher");
            entity.Property(e => e.Supporting).HasColumnName("supporting");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.CompanyNavigation).WithMany(p => p.InvolvedCompanies)
                .HasForeignKey(d => d.Company)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("involved_companies__dbt_tmp_company_fkey");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.InvolvedCompanies)
                .HasForeignKey(d => d.Game)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("involved_companies__dbt_tmp_game_fkey");
        });

        modelBuilder.Entity<Keyword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("keywords__dbt_tmp_pkey");

            entity.ToTable("keywords", "igdb");

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
            entity.HasKey(e => e.Id).HasName("languages__dbt_tmp_pkey1");

            entity.ToTable("languages", "igdb", tb => tb.HasComment("languages lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<LanguageSupport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("language_supports__dbt_tmp_pkey1");

            entity.ToTable("language_supports", "igdb");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Game)
                .HasComment("FK to mart_games.id — the game this language support belongs to.")
                .HasColumnName("game");
            entity.Property(e => e.Language)
                .HasComment("FK to languages.id — the supported language.")
                .HasColumnName("language");
            entity.Property(e => e.LanguageSupportType)
                .HasComment("FK to language_support_types.id — type of support (interface, subtitles, audio).")
                .HasColumnName("language_support_type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.LanguageSupports)
                .HasForeignKey(d => d.Game)
                .HasConstraintName("language_supports__dbt_tmp_game_fkey");

            entity.HasOne(d => d.LanguageNavigation).WithMany(p => p.LanguageSupports)
                .HasForeignKey(d => d.Language)
                .HasConstraintName("language_supports__dbt_tmp_language_fkey");

            entity.HasOne(d => d.LanguageSupportTypeNavigation).WithMany(p => p.LanguageSupports)
                .HasForeignKey(d => d.LanguageSupportType)
                .HasConstraintName("language_supports__dbt_tmp_language_support_type_fkey");
        });

        modelBuilder.Entity<LanguageSupportType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("language_support_types__dbt_tmp_pkey1");

            entity.ToTable("language_support_types", "igdb", tb => tb.HasComment("language_support_types lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<MultiplayerMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("multiplayer_modes__dbt_tmp_pkey1");

            entity.ToTable("multiplayer_modes", "igdb");

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

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.MultiplayerModesNavigation)
                .HasForeignKey(d => d.Game)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("multiplayer_modes__dbt_tmp_game_fkey");

            entity.HasOne(d => d.PlatformNavigation).WithMany(p => p.MultiplayerModes)
                .HasForeignKey(d => d.Platform)
                .HasConstraintName("multiplayer_modes__dbt_tmp_platform_fkey");
        });

        modelBuilder.Entity<NetworkType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("network_types__dbt_tmp_pkey");

            entity.ToTable("network_types", "igdb");

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

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platforms__dbt_tmp_pkey");

            entity.ToTable("platforms", "igdb");

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
            entity.HasKey(e => e.Id).HasName("platform_family__dbt_tmp_pkey1");

            entity.ToTable("platform_family", "igdb", tb => tb.HasComment("platform_family lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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
            entity.HasKey(e => e.Id).HasName("platform_logos__dbt_tmp_pkey1");

            entity.ToTable("platform_logos", "igdb");

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
            entity.HasKey(e => e.Id).HasName("platform_types__dbt_tmp_pkey1");

            entity.ToTable("platform_types", "igdb", tb => tb.HasComment("platform_types lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

            entity.ToTable("platform_versions", "igdb");

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

        modelBuilder.Entity<PlatformVersionCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_version_companies__dbt_tmp_pkey");

            entity.ToTable("platform_version_companies", "igdb");

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

            entity.HasOne(d => d.CompanyNavigation).WithMany(p => p.PlatformVersionCompanies)
                .HasForeignKey(d => d.Company)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("platform_version_companies__dbt_tmp_company_fkey");
        });

        modelBuilder.Entity<PlatformVersionReleaseDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_version_release_dates__dbt_tmp_pkey");

            entity.ToTable("platform_version_release_dates", "igdb", tb => tb.HasComment("Release dates for specific platform versions."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
                .HasColumnName("id");
            entity.Property(e => e.Checksum)
                .HasColumnType("character varying")
                .HasColumnName("checksum");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Date)
                .HasComment("Release date (Unix timestamp).")
                .HasColumnName("date");
            entity.Property(e => e.DateFormat)
                .HasComment("FK to date_formats.id.")
                .HasColumnName("date_format");
            entity.Property(e => e.Human)
                .HasComment("Human-readable release date.")
                .HasColumnType("character varying")
                .HasColumnName("human");
            entity.Property(e => e.M)
                .HasComment("Month.")
                .HasColumnName("m");
            entity.Property(e => e.ReleaseRegion)
                .HasComment("FK to release_date_regions.id.")
                .HasColumnName("release_region");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Y)
                .HasComment("Year.")
                .HasColumnName("y");
        });

        modelBuilder.Entity<PlatformWebsite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_websites__dbt_tmp_pkey1");

            entity.ToTable("platform_websites", "igdb");

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
            entity.HasKey(e => e.Id).HasName("player_perspectives__dbt_tmp_pkey1");

            entity.ToTable("player_perspectives", "igdb", tb => tb.HasComment("player_perspectives lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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
            entity.HasKey(e => new { e.GameId, e.PopularityType }).HasName("popularity_primitive__dbt_tmp_pkey1");

            entity.ToTable("popularity_primitive", "igdb");

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PopularityType).HasColumnName("popularity_type");
            entity.Property(e => e.CalculatedAt).HasColumnName("calculated_at");
            entity.Property(e => e.CapturedAt).HasColumnName("captured_at");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Game).WithMany(p => p.PopularityPrimitives)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("popularity_primitive__dbt_tmp_game_id_fkey");
        });

        modelBuilder.Entity<PopularityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("popularity_types__dbt_tmp_pkey");

            entity.ToTable("popularity_types", "igdb", tb => tb.HasComment("popularity_types lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("regions__dbt_tmp_pkey1");

            entity.ToTable("regions", "igdb", tb => tb.HasComment("regions lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

            entity.ToTable("release_dates", "igdb");

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
            entity.HasKey(e => e.Id).HasName("release_date_regions__dbt_tmp_pkey1");

            entity.ToTable("release_date_regions", "igdb", tb => tb.HasComment("release_date_regions lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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
            entity.HasKey(e => e.Id).HasName("release_date_statuses__dbt_tmp_pkey1");

            entity.ToTable("release_date_statuses", "igdb", tb => tb.HasComment("release_date_statuses lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

            entity.ToTable("screenshots", "igdb");

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

        modelBuilder.Entity<SteamDetail>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("steam_details__dbt_tmp_pkey1");

            entity.ToTable("steam_details", "igdb");

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("game_id");
            entity.Property(e => e.CapsuleUrl).HasColumnName("capsule_url");
            entity.Property(e => e.HeaderUrl).HasColumnName("header_url");
            entity.Property(e => e.SteamAppId).HasColumnName("steam_app_id");

            entity.HasOne(d => d.Game).WithOne(p => p.SteamDetail)
                .HasForeignKey<SteamDetail>(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("steam_details__dbt_tmp_game_id_fkey");
        });

        modelBuilder.Entity<SteamLatestPlayerCount>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("steam_latest_player_counts__dbt_tmp_pkey1");

            entity.ToTable("steam_latest_player_counts", "igdb");

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("game_id");
            entity.Property(e => e.CapturedAt).HasColumnName("captured_at");
            entity.Property(e => e.CurrentPlayers).HasColumnName("current_players");
            entity.Property(e => e.Peak24h).HasColumnName("peak_24h");
            entity.Property(e => e.Sparkline7d).HasColumnName("sparkline_7d");
            entity.Property(e => e.SteamAppId).HasColumnName("steam_app_id");

            entity.HasOne(d => d.Game).WithOne(p => p.SteamLatestPlayerCount)
                .HasForeignKey<SteamLatestPlayerCount>(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("steam_latest_player_counts__dbt_tmp_game_id_fkey");
        });

        modelBuilder.Entity<SteamLatestPricing>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("steam_latest_pricings__dbt_tmp_pkey1");

            entity.ToTable("steam_latest_pricings", "igdb");

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("game_id");
            entity.Property(e => e.CapturedAt).HasColumnName("captured_at");
            entity.Property(e => e.Currency).HasColumnName("currency");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.FinalCents).HasColumnName("final_cents");
            entity.Property(e => e.FinalFormatted).HasColumnName("final_formatted");
            entity.Property(e => e.High30d).HasColumnName("high_30d");
            entity.Property(e => e.InitialCents).HasColumnName("initial_cents");
            entity.Property(e => e.InitialFormatted).HasColumnName("initial_formatted");
            entity.Property(e => e.Low30d).HasColumnName("low_30d");
            entity.Property(e => e.SteamAppId).HasColumnName("steam_app_id");

            entity.HasOne(d => d.Game).WithOne(p => p.SteamLatestPricing)
                .HasForeignKey<SteamLatestPricing>(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("steam_latest_pricings__dbt_tmp_game_id_fkey");
        });

        modelBuilder.Entity<SteamPlayerStatsDaily>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("steam_player_stats_daily", "steam")
                .HasAnnotation("TimescaleDB:ChunkInterval", "240:00:00")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:EndOffset", "1 hour")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:HasRefreshPolicy", true)
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:ScheduleInterval", "1 day")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:StartOffset", "1 month")
                .HasAnnotation("TimescaleDB:MaterializedOnly", true)
                .HasAnnotation("TimescaleDB:MaterializedViewName", "steam_player_stats_daily")
                .HasAnnotation("TimescaleDB:ParentName", "concurrent_users")
                .HasAnnotation("TimescaleDB:ViewDefinition", " SELECT game_id,\n    time_bucket('1 day'::interval, captured_at) AS bucket,\n    (max(current_players))::integer AS peak_players,\n    (avg(current_players))::integer AS avg_players\n   FROM steam_raw.concurrent_users\n  GROUP BY game_id, (time_bucket('1 day'::interval, captured_at));");

            entity.Property(e => e.AvgPlayers).HasColumnName("avg_players");
            entity.Property(e => e.Bucket).HasColumnName("bucket");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PeakPlayers).HasColumnName("peak_players");
        });

        modelBuilder.Entity<SteamPlayerStatsHourly>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("steam_player_stats_hourly", "steam")
                .HasAnnotation("TimescaleDB:ChunkInterval", "240:00:00")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:EndOffset", "1 hour")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:HasRefreshPolicy", true)
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:ScheduleInterval", "01:00:00")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:StartOffset", "3 days")
                .HasAnnotation("TimescaleDB:MaterializedOnly", true)
                .HasAnnotation("TimescaleDB:MaterializedViewName", "steam_player_stats_hourly")
                .HasAnnotation("TimescaleDB:ParentName", "concurrent_users")
                .HasAnnotation("TimescaleDB:ViewDefinition", " SELECT game_id,\n    time_bucket('01:00:00'::interval, captured_at) AS bucket,\n    (max(current_players))::integer AS peak_players,\n    (avg(current_players))::integer AS avg_players\n   FROM steam_raw.concurrent_users\n  GROUP BY game_id, (time_bucket('01:00:00'::interval, captured_at));");

            entity.Property(e => e.AvgPlayers).HasColumnName("avg_players");
            entity.Property(e => e.Bucket).HasColumnName("bucket");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PeakPlayers).HasColumnName("peak_players");
        });

        modelBuilder.Entity<SteamPricingDaily>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("steam_pricing_daily", "steam")
                .HasAnnotation("TimescaleDB:ChunkInterval", "1680:00:00")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:EndOffset", "1 hour")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:HasRefreshPolicy", true)
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:ScheduleInterval", "1 day")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:StartOffset", "1 month")
                .HasAnnotation("TimescaleDB:MaterializedOnly", true)
                .HasAnnotation("TimescaleDB:MaterializedViewName", "steam_pricing_daily")
                .HasAnnotation("TimescaleDB:ParentName", "store_pricing")
                .HasAnnotation("TimescaleDB:ViewDefinition", " SELECT game_id,\n    time_bucket('1 day'::interval, captured_at) AS bucket,\n    min(final_cents) AS min_price,\n    max(final_cents) AS max_price,\n    (avg(final_cents))::integer AS avg_price\n   FROM steam_raw.store_pricing\n  GROUP BY game_id, (time_bucket('1 day'::interval, captured_at));");

            entity.Property(e => e.AvgPrice).HasColumnName("avg_price");
            entity.Property(e => e.Bucket).HasColumnName("bucket");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.MaxPrice).HasColumnName("max_price");
            entity.Property(e => e.MinPrice).HasColumnName("min_price");
        });

        modelBuilder.Entity<SteamPricingHourly>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("steam_pricing_hourly", "steam")
                .HasAnnotation("TimescaleDB:ChunkInterval", "1680:00:00")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:EndOffset", "1 hour")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:HasRefreshPolicy", true)
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:ScheduleInterval", "01:00:00")
                .HasAnnotation("TimescaleDB:ContinuousAggregatePolicy:StartOffset", "3 days")
                .HasAnnotation("TimescaleDB:MaterializedOnly", true)
                .HasAnnotation("TimescaleDB:MaterializedViewName", "steam_pricing_hourly")
                .HasAnnotation("TimescaleDB:ParentName", "store_pricing")
                .HasAnnotation("TimescaleDB:ViewDefinition", " SELECT game_id,\n    time_bucket('01:00:00'::interval, captured_at) AS bucket,\n    min(final_cents) AS min_price,\n    max(final_cents) AS max_price,\n    (avg(final_cents))::integer AS avg_price\n   FROM steam_raw.store_pricing\n  GROUP BY game_id, (time_bucket('01:00:00'::interval, captured_at));");

            entity.Property(e => e.AvgPrice).HasColumnName("avg_price");
            entity.Property(e => e.Bucket).HasColumnName("bucket");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.MaxPrice).HasColumnName("max_price");
            entity.Property(e => e.MinPrice).HasColumnName("min_price");
        });

        modelBuilder.Entity<SteamReview>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("steam_reviews__dbt_tmp_pkey1");

            entity.ToTable("steam_reviews", "igdb");

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("game_id");
            entity.Property(e => e.NumReviews).HasColumnName("num_reviews");
            entity.Property(e => e.ReviewScore).HasColumnName("review_score");
            entity.Property(e => e.ReviewScoreDesc).HasColumnName("review_score_desc");
            entity.Property(e => e.SteamAppId).HasColumnName("steam_app_id");
            entity.Property(e => e.TotalNegative).HasColumnName("total_negative");
            entity.Property(e => e.TotalPositive).HasColumnName("total_positive");
            entity.Property(e => e.TotalReviews).HasColumnName("total_reviews");

            entity.HasOne(d => d.Game).WithOne(p => p.SteamReview)
                .HasForeignKey<SteamReview>(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("steam_reviews__dbt_tmp_game_id_fkey");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("themes__dbt_tmp_pkey1");

            entity.ToTable("themes", "igdb", tb => tb.HasComment("themes lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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

        modelBuilder.Entity<TrackedGame>(entity =>
        {
            entity.HasKey(e => new { e.GameId, e.SteamAppId }).HasName("tracked_games_pkey");

            entity.ToTable("tracked_games", "steam");

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.SteamAppId).HasColumnName("steam_app_id");
            entity.Property(e => e.RefreshedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("refreshed_at");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("videos__dbt_tmp_pkey");

            entity.ToTable("videos", "igdb");

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

            entity.ToTable("websites", "igdb");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("websites__dbt_tmp_game_fkey");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Websites)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("websites__dbt_tmp_type_fkey");
        });

        modelBuilder.Entity<WebsiteType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("website_types__dbt_tmp_pkey1");

            entity.ToTable("website_types", "igdb", tb => tb.HasComment("website_types lookup table."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Primary key.")
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
