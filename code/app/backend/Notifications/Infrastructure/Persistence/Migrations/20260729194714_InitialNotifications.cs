using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Notifications.Infrastructure.Persistence.Migrations
{
    public partial class InitialNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "ticker");

            migrationBuilder.EnsureSchema(name: "notifications");

            migrationBuilder.CreateTable(
                name: "CronTickers",
                schema: "ticker",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    expression = table.Column<string>(type: "text", nullable: true),
                    request = table.Column<byte[]>(type: "bytea", nullable: true),
                    retries = table.Column<int>(type: "integer", nullable: false),
                    retry_intervals = table.Column<int[]>(type: "integer[]", nullable: true),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_system_paused = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    function = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    init_identifier = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cron_tickers", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "release_alert_events",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    release_day_utc = table.Column<DateOnly>(type: "date", nullable: false),
                    dispatch_at_utc = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    status = table.Column<string>(
                        type: "character varying(16)",
                        maxLength: 16,
                        nullable: false
                    ),
                    created_at_utc = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    updated_at_utc = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_release_alert_events", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "TimeTickers",
                schema: "ticker",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    function = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    init_identifier = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    status = table.Column<int>(type: "integer", nullable: false),
                    lock_holder = table.Column<string>(type: "text", nullable: true),
                    request = table.Column<byte[]>(type: "bytea", nullable: true),
                    execution_time = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    locked_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    executed_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    exception_message = table.Column<string>(type: "text", nullable: true),
                    skipped_reason = table.Column<string>(type: "text", nullable: true),
                    elapsed_time = table.Column<long>(type: "bigint", nullable: false),
                    retries = table.Column<int>(type: "integer", nullable: false),
                    retry_count = table.Column<int>(type: "integer", nullable: false),
                    retry_intervals = table.Column<int[]>(type: "integer[]", nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    run_condition = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_tickers", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_tickers_time_tickers_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "ticker",
                        principalTable: "TimeTickers",
                        principalColumn: "id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "CronTickerOccurrences",
                schema: "ticker",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    lock_holder = table.Column<string>(type: "text", nullable: true),
                    execution_time = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    cron_ticker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    locked_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    executed_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    exception_message = table.Column<string>(type: "text", nullable: true),
                    skipped_reason = table.Column<string>(type: "text", nullable: true),
                    elapsed_time = table.Column<long>(type: "bigint", nullable: false),
                    retry_count = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cron_ticker_occurrences", x => x.id);
                    table.ForeignKey(
                        name: "fk_cron_ticker_occurrences_cron_tickers_cron_ticker_id",
                        column: x => x.cron_ticker_id,
                        principalSchema: "ticker",
                        principalTable: "CronTickers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "notification_deliveries",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    release_alert_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    push_endpoint_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(
                        type: "character varying(16)",
                        maxLength: 16,
                        nullable: false
                    ),
                    status = table.Column<string>(
                        type: "character varying(16)",
                        maxLength: 16,
                        nullable: false
                    ),
                    created_at_utc = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    sent_at_utc = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_deliveries", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_deliveries_release_alert_events_release_alert_",
                        column: x => x.release_alert_event_id,
                        principalSchema: "notifications",
                        principalTable: "release_alert_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "release_alert_platforms",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    release_alert_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_release_date_id = table.Column<long>(type: "bigint", nullable: false),
                    platform_id = table.Column<long>(type: "bigint", nullable: false),
                    release_region_id = table.Column<long>(type: "bigint", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_release_alert_platforms", x => x.id);
                    table.ForeignKey(
                        name: "fk_release_alert_platforms_release_alert_events_release_alert_",
                        column: x => x.release_alert_event_id,
                        principalSchema: "notifications",
                        principalTable: "release_alert_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "notification_delivery_attempts",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_delivery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempted_at_utc = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    succeeded = table.Column<bool>(type: "boolean", nullable: false),
                    provider_receipt = table.Column<string>(type: "text", nullable: true),
                    failure_reason = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_delivery_attempts", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_delivery_attempts_notification_deliveries_noti",
                        column: x => x.notification_delivery_id,
                        principalSchema: "notifications",
                        principalTable: "notification_deliveries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_CronTickerOccurrence_CronTickerId",
                schema: "ticker",
                table: "CronTickerOccurrences",
                column: "cron_ticker_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_CronTickerOccurrence_ExecutionTime",
                schema: "ticker",
                table: "CronTickerOccurrences",
                column: "execution_time"
            );

            migrationBuilder.CreateIndex(
                name: "IX_CronTickerOccurrence_Status_ExecutionTime",
                schema: "ticker",
                table: "CronTickerOccurrences",
                columns: new[] { "status", "execution_time" }
            );

            migrationBuilder.CreateIndex(
                name: "UQ_CronTickerId_ExecutionTime",
                schema: "ticker",
                table: "CronTickerOccurrences",
                columns: new[] { "cron_ticker_id", "execution_time" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_CronTickers_Expression",
                schema: "ticker",
                table: "CronTickers",
                column: "expression"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Function_Expression",
                schema: "ticker",
                table: "CronTickers",
                columns: new[] { "function", "expression" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_notification_deliveries_release_alert_event_id_user_id_push",
                schema: "notifications",
                table: "notification_deliveries",
                columns: new[]
                {
                    "release_alert_event_id",
                    "user_id",
                    "push_endpoint_id",
                    "provider",
                },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_notification_deliveries_status_created_at_utc",
                schema: "notifications",
                table: "notification_deliveries",
                columns: new[] { "status", "created_at_utc" }
            );

            migrationBuilder.CreateIndex(
                name: "ix_notification_delivery_attempts_notification_delivery_id",
                schema: "notifications",
                table: "notification_delivery_attempts",
                column: "notification_delivery_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_release_alert_events_game_id_release_day_utc",
                schema: "notifications",
                table: "release_alert_events",
                columns: new[] { "game_id", "release_day_utc" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_release_alert_platforms_release_alert_event_id_source_relea",
                schema: "notifications",
                table: "release_alert_platforms",
                columns: new[] { "release_alert_event_id", "source_release_date_id" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_time_tickers_parent_id",
                schema: "ticker",
                table: "TimeTickers",
                column: "parent_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_TimeTicker_ExecutionTime",
                schema: "ticker",
                table: "TimeTickers",
                column: "execution_time"
            );

            migrationBuilder.CreateIndex(
                name: "IX_TimeTicker_Status_ExecutionTime",
                schema: "ticker",
                table: "TimeTickers",
                columns: new[] { "status", "execution_time" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CronTickerOccurrences", schema: "ticker");

            migrationBuilder.DropTable(
                name: "notification_delivery_attempts",
                schema: "notifications"
            );

            migrationBuilder.DropTable(name: "release_alert_platforms", schema: "notifications");

            migrationBuilder.DropTable(name: "TimeTickers", schema: "ticker");

            migrationBuilder.DropTable(name: "CronTickers", schema: "ticker");

            migrationBuilder.DropTable(name: "notification_deliveries", schema: "notifications");

            migrationBuilder.DropTable(name: "release_alert_events", schema: "notifications");
        }
    }
}
