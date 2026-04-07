using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "game_visit_counts",
                columns: new[] { "id", "count", "game_id", "last_visited_at" },
                values: new object[] { 1, 0L, -1L, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "game_visit_counts",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
