using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Play.Infrastructure.Persistence;

namespace PlayAPI.Infrastructure.Persistence.Migrations;

[DbContext(typeof(PlayDbContext))]
[Migration("20260728090000_AddUserLibraryKeysetIndexes")]
public partial class AddUserLibraryKeysetIndexes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "ix_list_history_user_id_created_at_id",
            table: "list_history",
            columns: new[] { "user_id", "created_at", "id" },
            descending: new[] { false, true, true }
        );

        migrationBuilder.CreateIndex(
            name: "ix_list_items_list_id_added_at_game_id",
            table: "list_items",
            columns: new[] { "list_id", "added_at", "game_id" },
            descending: new[] { false, true, false }
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_list_history_user_id_created_at_id",
            table: "list_history"
        );

        migrationBuilder.DropIndex(
            name: "ix_list_items_list_id_added_at_game_id",
            table: "list_items"
        );
    }
}
