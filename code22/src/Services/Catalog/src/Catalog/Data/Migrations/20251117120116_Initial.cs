using IGDB.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catalog.Data.Migrations
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
                name: "companies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<Company>(type: "jsonb", nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    changed_company_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_companies", x => x.id);
                    table.ForeignKey(
                        name: "fk_companies_companies_changed_company_id",
                        column: x => x.changed_company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_companies_companies_parent_id",
                        column: x => x.parent_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "franchises",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<Franchise>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_franchises", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_engines",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<GameEngine>(type: "jsonb", nullable: false),
                    logo = table.Column<GameEngineLogo>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_engines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_modes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    game_type = table.Column<long>(type: "bigint", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    rating_count = table.Column<long>(type: "bigint", nullable: true),
                    total_rating = table.Column<double>(type: "double precision", nullable: true),
                    total_rating_count = table.Column<long>(type: "bigint", nullable: true),
                    url = table.Column<string>(type: "text", nullable: false),
                    first_release_date = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<Game>(type: "jsonb", nullable: false),
                    game_entity_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                    table.ForeignKey(
                        name: "fk_games_games_game_entity_id",
                        column: x => x.game_entity_id,
                        principalTable: "games",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platforms",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    abbreviation = table.Column<string>(type: "text", nullable: true),
                    generation = table.Column<long>(type: "bigint", nullable: true),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    logo = table.Column<PlatformLogo>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_platforms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player_perspectives",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_perspectives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_themes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "developer_game",
                columns: table => new
                {
                    company_id = table.Column<long>(type: "bigint", nullable: false),
                    game_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_developer_game", x => new { x.company_id, x.game_id });
                    table.ForeignKey(
                        name: "fk_developer_game_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_developer_game_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "franchise_entity_game_entity",
                columns: table => new
                {
                    franchises_id = table.Column<long>(type: "bigint", nullable: false),
                    games_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_franchise_entity_game_entity", x => new { x.franchises_id, x.games_id });
                    table.ForeignKey(
                        name: "fk_franchise_entity_game_entity_franchises_franchises_id",
                        column: x => x.franchises_id,
                        principalTable: "franchises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_franchise_entity_game_entity_games_games_id",
                        column: x => x.games_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_engine_entity_game_entity",
                columns: table => new
                {
                    game_engines_id = table.Column<long>(type: "bigint", nullable: false),
                    games_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_engine_entity_game_entity", x => new { x.game_engines_id, x.games_id });
                    table.ForeignKey(
                        name: "fk_game_engine_entity_game_entity_game_engines_game_engines_id",
                        column: x => x.game_engines_id,
                        principalTable: "game_engines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_engine_entity_game_entity_games_games_id",
                        column: x => x.games_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_entity_game_mode_entity",
                columns: table => new
                {
                    game_modes_id = table.Column<long>(type: "bigint", nullable: false),
                    games_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_entity_game_mode_entity", x => new { x.game_modes_id, x.games_id });
                    table.ForeignKey(
                        name: "fk_game_entity_game_mode_entity_game_modes_game_modes_id",
                        column: x => x.game_modes_id,
                        principalTable: "game_modes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_entity_game_mode_entity_games_games_id",
                        column: x => x.games_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publisher_game",
                columns: table => new
                {
                    company_id = table.Column<long>(type: "bigint", nullable: false),
                    game_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_publisher_game", x => new { x.company_id, x.game_id });
                    table.ForeignKey(
                        name: "fk_publisher_game_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_publisher_game_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_entity_genre_entity",
                columns: table => new
                {
                    games_id = table.Column<long>(type: "bigint", nullable: false),
                    genres_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_entity_genre_entity", x => new { x.games_id, x.genres_id });
                    table.ForeignKey(
                        name: "fk_game_entity_genre_entity_games_games_id",
                        column: x => x.games_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_entity_genre_entity_genres_genres_id",
                        column: x => x.genres_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_entity_platform_entity",
                columns: table => new
                {
                    games_id = table.Column<long>(type: "bigint", nullable: false),
                    platforms_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_entity_platform_entity", x => new { x.games_id, x.platforms_id });
                    table.ForeignKey(
                        name: "fk_game_entity_platform_entity_games_games_id",
                        column: x => x.games_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_entity_platform_entity_platforms_platforms_id",
                        column: x => x.platforms_id,
                        principalTable: "platforms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_entity_player_perspective_entity",
                columns: table => new
                {
                    games_id = table.Column<long>(type: "bigint", nullable: false),
                    player_perspectives_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_entity_player_perspective_entity", x => new { x.games_id, x.player_perspectives_id });
                    table.ForeignKey(
                        name: "fk_game_entity_player_perspective_entity_games_games_id",
                        column: x => x.games_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_entity_player_perspective_entity_player_perspectives_p",
                        column: x => x.player_perspectives_id,
                        principalTable: "player_perspectives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_entity_theme_entity",
                columns: table => new
                {
                    games_id = table.Column<long>(type: "bigint", nullable: false),
                    themes_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_entity_theme_entity", x => new { x.games_id, x.themes_id });
                    table.ForeignKey(
                        name: "fk_game_entity_theme_entity_games_games_id",
                        column: x => x.games_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_entity_theme_entity_themes_themes_id",
                        column: x => x.themes_id,
                        principalTable: "themes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_companies_changed_company_id",
                table: "companies",
                column: "changed_company_id");

            migrationBuilder.CreateIndex(
                name: "ix_companies_parent_id",
                table: "companies",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_developer_game_game_id",
                table: "developer_game",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_franchise_entity_game_entity_games_id",
                table: "franchise_entity_game_entity",
                column: "games_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_engine_entity_game_entity_games_id",
                table: "game_engine_entity_game_entity",
                column: "games_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_entity_game_mode_entity_games_id",
                table: "game_entity_game_mode_entity",
                column: "games_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_entity_genre_entity_genres_id",
                table: "game_entity_genre_entity",
                column: "genres_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_entity_platform_entity_platforms_id",
                table: "game_entity_platform_entity",
                column: "platforms_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_entity_player_perspective_entity_player_perspectives_id",
                table: "game_entity_player_perspective_entity",
                column: "player_perspectives_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_entity_theme_entity_themes_id",
                table: "game_entity_theme_entity",
                column: "themes_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_game_entity_id",
                table: "games",
                column: "game_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_publisher_game_game_id",
                table: "publisher_game",
                column: "game_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "developer_game");

            migrationBuilder.DropTable(
                name: "franchise_entity_game_entity");

            migrationBuilder.DropTable(
                name: "game_engine_entity_game_entity");

            migrationBuilder.DropTable(
                name: "game_entity_game_mode_entity");

            migrationBuilder.DropTable(
                name: "game_entity_genre_entity");

            migrationBuilder.DropTable(
                name: "game_entity_platform_entity");

            migrationBuilder.DropTable(
                name: "game_entity_player_perspective_entity");

            migrationBuilder.DropTable(
                name: "game_entity_theme_entity");

            migrationBuilder.DropTable(
                name: "publisher_game");

            migrationBuilder.DropTable(
                name: "franchises");

            migrationBuilder.DropTable(
                name: "game_engines");

            migrationBuilder.DropTable(
                name: "game_modes");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "platforms");

            migrationBuilder.DropTable(
                name: "player_perspectives");

            migrationBuilder.DropTable(
                name: "themes");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "games");
        }
    }
}
