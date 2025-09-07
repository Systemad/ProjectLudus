using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Shared.Features;

#nullable disable

namespace Worker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    release_date = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    game_type = table.Column<long>(type: "bigint", nullable: false),
                    platforms = table.Column<long[]>(type: "bigint[]", nullable: false),
                    game_engines = table.Column<long[]>(type: "bigint[]", nullable: false),
                    genres = table.Column<long[]>(type: "bigint[]", nullable: false),
                    themes = table.Column<long[]>(type: "bigint[]", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: false),
                    rating_count = table.Column<double>(type: "double precision", nullable: false),
                    total_rating = table.Column<double>(type: "double precision", nullable: false),
                    total_rating_count = table.Column<double>(type: "double precision", nullable: false),
                    raw_data = table.Column<IGDBGame>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");
        }
    }
}
