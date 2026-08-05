using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayAPI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlignAspireModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_auth_sessions_users_user_id",
                table: "auth_sessions"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_auth_transactions_users_user_id",
                table: "auth_transactions"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_list_history_lists_list_id",
                table: "list_history"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_list_history_users_user_id",
                table: "list_history"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_list_items_lists_list_id",
                table: "list_items"
            );

            migrationBuilder.DropForeignKey(name: "fk_lists_users_user_id", table: "lists");

            migrationBuilder.DropPrimaryKey(name: "pk_users", table: "users");

            migrationBuilder.DropPrimaryKey(name: "pk_lists", table: "lists");

            migrationBuilder.DropPrimaryKey(name: "pk_list_items", table: "list_items");

            migrationBuilder.DropPrimaryKey(name: "pk_list_history", table: "list_history");

            migrationBuilder.DropPrimaryKey(
                name: "pk_auth_transactions",
                table: "auth_transactions"
            );

            migrationBuilder.DropPrimaryKey(name: "pk_auth_sessions", table: "auth_sessions");

            migrationBuilder.RenameTable(name: "users", newName: "Users");

            migrationBuilder.RenameTable(name: "lists", newName: "Lists");

            migrationBuilder.RenameTable(name: "list_items", newName: "ListItems");

            migrationBuilder.RenameTable(name: "list_history", newName: "ListHistory");

            migrationBuilder.RenameTable(name: "auth_transactions", newName: "AuthTransactions");

            migrationBuilder.RenameTable(name: "auth_sessions", newName: "AuthSessions");

            migrationBuilder.RenameColumn(name: "role", table: "Users", newName: "Role");

            migrationBuilder.RenameColumn(name: "id", table: "Users", newName: "Id");

            migrationBuilder.RenameColumn(name: "updated_at", table: "Users", newName: "UpdatedAt");

            migrationBuilder.RenameColumn(name: "steam_name", table: "Users", newName: "SteamName");

            migrationBuilder.RenameColumn(name: "steam_id", table: "Users", newName: "SteamId");

            migrationBuilder.RenameColumn(name: "created_at", table: "Users", newName: "CreatedAt");

            migrationBuilder.RenameColumn(name: "avatar_url", table: "Users", newName: "AvatarUrl");

            migrationBuilder.RenameIndex(
                name: "ix_users_steam_id",
                table: "Users",
                newName: "IX_Users_SteamId"
            );

            migrationBuilder.RenameColumn(
                name: "visibility",
                table: "Lists",
                newName: "Visibility"
            );

            migrationBuilder.RenameColumn(name: "name", table: "Lists", newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Lists",
                newName: "Description"
            );

            migrationBuilder.RenameColumn(name: "id", table: "Lists", newName: "Id");

            migrationBuilder.RenameColumn(name: "user_id", table: "Lists", newName: "UserId");

            migrationBuilder.RenameColumn(name: "updated_at", table: "Lists", newName: "UpdatedAt");

            migrationBuilder.RenameColumn(name: "is_default", table: "Lists", newName: "IsDefault");

            migrationBuilder.RenameColumn(name: "created_at", table: "Lists", newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_lists_user_id_name",
                table: "Lists",
                newName: "IX_Lists_UserId_Name"
            );

            migrationBuilder.RenameColumn(name: "added_at", table: "ListItems", newName: "AddedAt");

            migrationBuilder.RenameColumn(name: "game_id", table: "ListItems", newName: "GameId");

            migrationBuilder.RenameColumn(name: "list_id", table: "ListItems", newName: "ListId");

            migrationBuilder.RenameIndex(
                name: "ix_list_items_game_id",
                table: "ListItems",
                newName: "IX_ListItems_GameId"
            );

            migrationBuilder.RenameColumn(name: "action", table: "ListHistory", newName: "Action");

            migrationBuilder.RenameColumn(name: "id", table: "ListHistory", newName: "Id");

            migrationBuilder.RenameColumn(name: "user_id", table: "ListHistory", newName: "UserId");

            migrationBuilder.RenameColumn(name: "list_id", table: "ListHistory", newName: "ListId");

            migrationBuilder.RenameColumn(name: "game_id", table: "ListHistory", newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ListHistory",
                newName: "CreatedAt"
            );

            migrationBuilder.RenameIndex(
                name: "ix_list_history_user_id",
                table: "ListHistory",
                newName: "IX_ListHistory_UserId"
            );

            migrationBuilder.RenameIndex(
                name: "ix_list_history_list_id",
                table: "ListHistory",
                newName: "IX_ListHistory_ListId"
            );

            migrationBuilder.RenameColumn(
                name: "state",
                table: "AuthTransactions",
                newName: "State"
            );

            migrationBuilder.RenameColumn(name: "id", table: "AuthTransactions", newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AuthTransactions",
                newName: "UserId"
            );

            migrationBuilder.RenameColumn(
                name: "expires_at",
                table: "AuthTransactions",
                newName: "ExpiresAt"
            );

            migrationBuilder.RenameColumn(
                name: "consumed_at",
                table: "AuthTransactions",
                newName: "ConsumedAt"
            );

            migrationBuilder.RenameColumn(
                name: "code_hash",
                table: "AuthTransactions",
                newName: "CodeHash"
            );

            migrationBuilder.RenameIndex(
                name: "ix_auth_transactions_user_id",
                table: "AuthTransactions",
                newName: "IX_AuthTransactions_UserId"
            );

            migrationBuilder.RenameIndex(
                name: "ix_auth_transactions_code_hash",
                table: "AuthTransactions",
                newName: "IX_AuthTransactions_CodeHash"
            );

            migrationBuilder.RenameColumn(name: "id", table: "AuthSessions", newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AuthSessions",
                newName: "UserId"
            );

            migrationBuilder.RenameColumn(
                name: "token_hash",
                table: "AuthSessions",
                newName: "TokenHash"
            );

            migrationBuilder.RenameColumn(
                name: "revoked_at",
                table: "AuthSessions",
                newName: "RevokedAt"
            );

            migrationBuilder.RenameColumn(
                name: "expires_at",
                table: "AuthSessions",
                newName: "ExpiresAt"
            );

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "AuthSessions",
                newName: "CreatedAt"
            );

            migrationBuilder.RenameIndex(
                name: "ix_auth_sessions_user_id",
                table: "AuthSessions",
                newName: "IX_AuthSessions_UserId"
            );

            migrationBuilder.RenameIndex(
                name: "ix_auth_sessions_token_hash",
                table: "AuthSessions",
                newName: "IX_AuthSessions_TokenHash"
            );

            migrationBuilder.AddPrimaryKey(name: "PK_Users", table: "Users", column: "Id");

            migrationBuilder.AddPrimaryKey(name: "PK_Lists", table: "Lists", column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListItems",
                table: "ListItems",
                columns: new[] { "ListId", "GameId" }
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListHistory",
                table: "ListHistory",
                column: "Id"
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthTransactions",
                table: "AuthTransactions",
                column: "Id"
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthSessions",
                table: "AuthSessions",
                column: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AuthSessions_Users_UserId",
                table: "AuthSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AuthTransactions_Users_UserId",
                table: "AuthTransactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ListHistory_Lists_ListId",
                table: "ListHistory",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ListHistory_Users_UserId",
                table: "ListHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_Lists_ListId",
                table: "ListItems",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Users_UserId",
                table: "Lists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthSessions_Users_UserId",
                table: "AuthSessions"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AuthTransactions_Users_UserId",
                table: "AuthTransactions"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ListHistory_Lists_ListId",
                table: "ListHistory"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ListHistory_Users_UserId",
                table: "ListHistory"
            );

            migrationBuilder.DropForeignKey(name: "FK_ListItems_Lists_ListId", table: "ListItems");

            migrationBuilder.DropForeignKey(name: "FK_Lists_Users_UserId", table: "Lists");

            migrationBuilder.DropPrimaryKey(name: "PK_Users", table: "Users");

            migrationBuilder.DropPrimaryKey(name: "PK_Lists", table: "Lists");

            migrationBuilder.DropPrimaryKey(name: "PK_ListItems", table: "ListItems");

            migrationBuilder.DropPrimaryKey(name: "PK_ListHistory", table: "ListHistory");

            migrationBuilder.DropPrimaryKey(name: "PK_AuthTransactions", table: "AuthTransactions");

            migrationBuilder.DropPrimaryKey(name: "PK_AuthSessions", table: "AuthSessions");

            migrationBuilder.RenameTable(name: "Users", newName: "users");

            migrationBuilder.RenameTable(name: "Lists", newName: "lists");

            migrationBuilder.RenameTable(name: "ListItems", newName: "list_items");

            migrationBuilder.RenameTable(name: "ListHistory", newName: "list_history");

            migrationBuilder.RenameTable(name: "AuthTransactions", newName: "auth_transactions");

            migrationBuilder.RenameTable(name: "AuthSessions", newName: "auth_sessions");

            migrationBuilder.RenameColumn(name: "Role", table: "users", newName: "role");

            migrationBuilder.RenameColumn(name: "Id", table: "users", newName: "id");

            migrationBuilder.RenameColumn(name: "UpdatedAt", table: "users", newName: "updated_at");

            migrationBuilder.RenameColumn(name: "SteamName", table: "users", newName: "steam_name");

            migrationBuilder.RenameColumn(name: "SteamId", table: "users", newName: "steam_id");

            migrationBuilder.RenameColumn(name: "CreatedAt", table: "users", newName: "created_at");

            migrationBuilder.RenameColumn(name: "AvatarUrl", table: "users", newName: "avatar_url");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SteamId",
                table: "users",
                newName: "ix_users_steam_id"
            );

            migrationBuilder.RenameColumn(
                name: "Visibility",
                table: "lists",
                newName: "visibility"
            );

            migrationBuilder.RenameColumn(name: "Name", table: "lists", newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "lists",
                newName: "description"
            );

            migrationBuilder.RenameColumn(name: "Id", table: "lists", newName: "id");

            migrationBuilder.RenameColumn(name: "UserId", table: "lists", newName: "user_id");

            migrationBuilder.RenameColumn(name: "UpdatedAt", table: "lists", newName: "updated_at");

            migrationBuilder.RenameColumn(name: "IsDefault", table: "lists", newName: "is_default");

            migrationBuilder.RenameColumn(name: "CreatedAt", table: "lists", newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Lists_UserId_Name",
                table: "lists",
                newName: "ix_lists_user_id_name"
            );

            migrationBuilder.RenameColumn(
                name: "AddedAt",
                table: "list_items",
                newName: "added_at"
            );

            migrationBuilder.RenameColumn(name: "GameId", table: "list_items", newName: "game_id");

            migrationBuilder.RenameColumn(name: "ListId", table: "list_items", newName: "list_id");

            migrationBuilder.RenameIndex(
                name: "IX_ListItems_GameId",
                table: "list_items",
                newName: "ix_list_items_game_id"
            );

            migrationBuilder.RenameColumn(name: "Action", table: "list_history", newName: "action");

            migrationBuilder.RenameColumn(name: "Id", table: "list_history", newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "list_history",
                newName: "user_id"
            );

            migrationBuilder.RenameColumn(
                name: "ListId",
                table: "list_history",
                newName: "list_id"
            );

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "list_history",
                newName: "game_id"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "list_history",
                newName: "created_at"
            );

            migrationBuilder.RenameIndex(
                name: "IX_ListHistory_UserId",
                table: "list_history",
                newName: "ix_list_history_user_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_ListHistory_ListId",
                table: "list_history",
                newName: "ix_list_history_list_id"
            );

            migrationBuilder.RenameColumn(
                name: "State",
                table: "auth_transactions",
                newName: "state"
            );

            migrationBuilder.RenameColumn(name: "Id", table: "auth_transactions", newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "auth_transactions",
                newName: "user_id"
            );

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "auth_transactions",
                newName: "expires_at"
            );

            migrationBuilder.RenameColumn(
                name: "ConsumedAt",
                table: "auth_transactions",
                newName: "consumed_at"
            );

            migrationBuilder.RenameColumn(
                name: "CodeHash",
                table: "auth_transactions",
                newName: "code_hash"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AuthTransactions_UserId",
                table: "auth_transactions",
                newName: "ix_auth_transactions_user_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AuthTransactions_CodeHash",
                table: "auth_transactions",
                newName: "ix_auth_transactions_code_hash"
            );

            migrationBuilder.RenameColumn(name: "Id", table: "auth_sessions", newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "auth_sessions",
                newName: "user_id"
            );

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "auth_sessions",
                newName: "token_hash"
            );

            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                table: "auth_sessions",
                newName: "revoked_at"
            );

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "auth_sessions",
                newName: "expires_at"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "auth_sessions",
                newName: "created_at"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AuthSessions_UserId",
                table: "auth_sessions",
                newName: "ix_auth_sessions_user_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AuthSessions_TokenHash",
                table: "auth_sessions",
                newName: "ix_auth_sessions_token_hash"
            );

            migrationBuilder.AddPrimaryKey(name: "pk_users", table: "users", column: "id");

            migrationBuilder.AddPrimaryKey(name: "pk_lists", table: "lists", column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_list_items",
                table: "list_items",
                columns: new[] { "list_id", "game_id" }
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_list_history",
                table: "list_history",
                column: "id"
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_auth_transactions",
                table: "auth_transactions",
                column: "id"
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_auth_sessions",
                table: "auth_sessions",
                column: "id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_auth_sessions_users_user_id",
                table: "auth_sessions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_auth_transactions_users_user_id",
                table: "auth_transactions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_list_history_lists_list_id",
                table: "list_history",
                column: "list_id",
                principalTable: "lists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_list_history_users_user_id",
                table: "list_history",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "fk_list_items_lists_list_id",
                table: "list_items",
                column: "list_id",
                principalTable: "lists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_lists_users_user_id",
                table: "lists",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
