using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PlayAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    event_type = table.Column<int>(type: "integer", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_metrics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    first_visited_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    last_visited_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    view_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_metrics", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_events");

            migrationBuilder.DropTable(
                name: "game_metrics");
        }
    }
}
