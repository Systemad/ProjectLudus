using IGDB.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CatalogAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_search", ",,");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Added = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<Company>(type: "jsonb", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    ChangedCompanyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Companies_ChangedCompanyId",
                        column: x => x.ChangedCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Companies_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Franchises",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Metadata = table.Column<Franchise>(type: "jsonb", nullable: false),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Added = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameEngines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Metadata = table.Column<GameEngine>(type: "jsonb", nullable: false),
                    Logo = table.Column<GameEngineLogo>(type: "jsonb", nullable: true),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEngines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameModes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Generation = table.Column<long>(type: "bigint", nullable: true),
                    Logo = table.Column<PlatformLogo>(type: "jsonb", nullable: true),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerPerspectiveEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPerspectiveEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GameType = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: true),
                    RatingCount = table.Column<long>(type: "bigint", nullable: true),
                    TotalRating = table.Column<double>(type: "double precision", nullable: true),
                    TotalRatingCount = table.Column<long>(type: "bigint", nullable: true),
                    FirstReleaseDate = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<Game>(type: "jsonb", nullable: false),
                    FranchiseEntityId = table.Column<long>(type: "bigint", nullable: true),
                    GameEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Franchises_FranchiseEntityId",
                        column: x => x.FranchiseEntityId,
                        principalTable: "Franchises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Games_GameEntityId",
                        column: x => x.GameEntityId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeveloperGame",
                columns: table => new
                {
                    DevelopedId = table.Column<long>(type: "bigint", nullable: false),
                    DevelopersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeveloperGame", x => new { x.DevelopedId, x.DevelopersId });
                    table.ForeignKey(
                        name: "FK_DeveloperGame_Companies_DevelopersId",
                        column: x => x.DevelopersId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeveloperGame_Games_DevelopedId",
                        column: x => x.DevelopedId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEngineEntityGameEntity",
                columns: table => new
                {
                    GameEnginesId = table.Column<long>(type: "bigint", nullable: false),
                    GamesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEngineEntityGameEntity", x => new { x.GameEnginesId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_GameEngineEntityGameEntity_GameEngines_GameEnginesId",
                        column: x => x.GameEnginesId,
                        principalTable: "GameEngines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEngineEntityGameEntity_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEntityGameModeEntity",
                columns: table => new
                {
                    GameModesId = table.Column<long>(type: "bigint", nullable: false),
                    GamesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEntityGameModeEntity", x => new { x.GameModesId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_GameEntityGameModeEntity_GameModes_GameModesId",
                        column: x => x.GameModesId,
                        principalTable: "GameModes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEntityGameModeEntity_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEntityGenreEntity",
                columns: table => new
                {
                    GenresId = table.Column<long>(type: "bigint", nullable: false),
                    GenresId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEntityGenreEntity", x => new { x.GenresId, x.GenresId1 });
                    table.ForeignKey(
                        name: "FK_GameEntityGenreEntity_Games_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEntityGenreEntity_Genres_GenresId1",
                        column: x => x.GenresId1,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEntityPlatformEntity",
                columns: table => new
                {
                    GamesId = table.Column<long>(type: "bigint", nullable: false),
                    PlatformsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEntityPlatformEntity", x => new { x.GamesId, x.PlatformsId });
                    table.ForeignKey(
                        name: "FK_GameEntityPlatformEntity_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEntityPlatformEntity_Platforms_PlatformsId",
                        column: x => x.PlatformsId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEntityPlayerPerspectiveEntity",
                columns: table => new
                {
                    GamesId = table.Column<long>(type: "bigint", nullable: false),
                    PlayerPerspectivesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEntityPlayerPerspectiveEntity", x => new { x.GamesId, x.PlayerPerspectivesId });
                    table.ForeignKey(
                        name: "FK_GameEntityPlayerPerspectiveEntity_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEntityPlayerPerspectiveEntity_PlayerPerspectiveEntity_P~",
                        column: x => x.PlayerPerspectivesId,
                        principalTable: "PlayerPerspectiveEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEntityThemeEntity",
                columns: table => new
                {
                    GamesId = table.Column<long>(type: "bigint", nullable: false),
                    ThemesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEntityThemeEntity", x => new { x.GamesId, x.ThemesId });
                    table.ForeignKey(
                        name: "FK_GameEntityThemeEntity_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEntityThemeEntity_Themes_ThemesId",
                        column: x => x.ThemesId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublisherGame",
                columns: table => new
                {
                    PublishedId = table.Column<long>(type: "bigint", nullable: false),
                    PublishersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublisherGame", x => new { x.PublishedId, x.PublishersId });
                    table.ForeignKey(
                        name: "FK_PublisherGame_Companies_PublishersId",
                        column: x => x.PublishersId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublisherGame_Games_PublishedId",
                        column: x => x.PublishedId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ChangedCompanyId",
                table: "Companies",
                column: "ChangedCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ParentId",
                table: "Companies",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeveloperGame_DevelopersId",
                table: "DeveloperGame",
                column: "DevelopersId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngineEntityGameEntity_GamesId",
                table: "GameEngineEntityGameEntity",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEntityGameModeEntity_GamesId",
                table: "GameEntityGameModeEntity",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEntityGenreEntity_GenresId1",
                table: "GameEntityGenreEntity",
                column: "GenresId1");

            migrationBuilder.CreateIndex(
                name: "IX_GameEntityPlatformEntity_PlatformsId",
                table: "GameEntityPlatformEntity",
                column: "PlatformsId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEntityPlayerPerspectiveEntity_PlayerPerspectivesId",
                table: "GameEntityPlayerPerspectiveEntity",
                column: "PlayerPerspectivesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEntityThemeEntity_ThemesId",
                table: "GameEntityThemeEntity",
                column: "ThemesId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_FranchiseEntityId",
                table: "Games",
                column: "FranchiseEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameEntityId",
                table: "Games",
                column: "GameEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherGame_PublishersId",
                table: "PublisherGame",
                column: "PublishersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeveloperGame");

            migrationBuilder.DropTable(
                name: "GameEngineEntityGameEntity");

            migrationBuilder.DropTable(
                name: "GameEntityGameModeEntity");

            migrationBuilder.DropTable(
                name: "GameEntityGenreEntity");

            migrationBuilder.DropTable(
                name: "GameEntityPlatformEntity");

            migrationBuilder.DropTable(
                name: "GameEntityPlayerPerspectiveEntity");

            migrationBuilder.DropTable(
                name: "GameEntityThemeEntity");

            migrationBuilder.DropTable(
                name: "PublisherGame");

            migrationBuilder.DropTable(
                name: "GameEngines");

            migrationBuilder.DropTable(
                name: "GameModes");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "PlayerPerspectiveEntity");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Franchises");
        }
    }
}
