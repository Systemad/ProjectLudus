using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlignPlayModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_list_history_user_id",
                table: "list_history");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_list_history_user_id",
                table: "list_history",
                column: "user_id");
        }
    }
}
