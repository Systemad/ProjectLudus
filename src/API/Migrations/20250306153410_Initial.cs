using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgeRating",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Organization = table.Column<long>(type: "bigint", nullable: false),
                    RatingCategory = table.Column<long>(type: "bigint", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeRating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cover",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageId = table.Column<string>(type: "text", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true),
                    GameId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cover", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoverId = table.Column<long>(type: "bigint", nullable: false),
                    FirstReleaseDate = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    GameType = table.Column<long>(type: "bigint", nullable: false),
                    Storyline = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Cover_CoverId",
                        column: x => x.CoverId,
                        principalTable: "Cover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEngine",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true),
                    GameId1 = table.Column<long>(type: "bigint", nullable: true),
                    GameId2 = table.Column<long>(type: "bigint", nullable: true),
                    GameId3 = table.Column<long>(type: "bigint", nullable: true),
                    GameId4 = table.Column<long>(type: "bigint", nullable: true),
                    GameId5 = table.Column<long>(type: "bigint", nullable: true),
                    GameId6 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEngine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId1",
                        column: x => x.GameId1,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId2",
                        column: x => x.GameId2,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId3",
                        column: x => x.GameId3,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId4",
                        column: x => x.GameId4,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId5",
                        column: x => x.GameId5,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId6",
                        column: x => x.GameId6,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SimilarGame",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoverId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimilarGame_Cover_CoverId",
                        column: x => x.CoverId,
                        principalTable: "Cover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimilarGame_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Website",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<long>(type: "bigint", nullable: false),
                    Game = table.Column<long>(type: "bigint", nullable: false),
                    Trusted = table.Column<bool>(type: "boolean", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Checksum = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<long>(type: "bigint", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Website_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvolvedCompany",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Developer = table.Column<bool>(type: "boolean", nullable: false),
                    Game = table.Column<long>(type: "bigint", nullable: false),
                    Porting = table.Column<bool>(type: "boolean", nullable: false),
                    Publisher = table.Column<bool>(type: "boolean", nullable: false),
                    Supporting = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Checksum = table.Column<string>(type: "text", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvolvedCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvolvedCompany_GameEngine_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "GameEngine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvolvedCompany_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReleaseDate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<long>(type: "bigint", nullable: true),
                    Game = table.Column<long>(type: "bigint", nullable: false),
                    Human = table.Column<string>(type: "text", nullable: false),
                    M = table.Column<long>(type: "bigint", nullable: true),
                    PlatformId = table.Column<long>(type: "bigint", nullable: false),
                    Region = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Y = table.Column<long>(type: "bigint", nullable: true),
                    Checksum = table.Column<string>(type: "text", nullable: false),
                    DateFormat = table.Column<long>(type: "bigint", nullable: false),
                    ReleaseRegion = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<long>(type: "bigint", nullable: true),
                    GameId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseDate_GameEngine_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "GameEngine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseDate_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgeRating_GameId",
                table: "AgeRating",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Cover_GameId",
                table: "Cover",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Cover_GameId1",
                table: "Cover",
                column: "GameId1");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId",
                table: "GameEngine",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId1",
                table: "GameEngine",
                column: "GameId1");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId2",
                table: "GameEngine",
                column: "GameId2");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId3",
                table: "GameEngine",
                column: "GameId3");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId4",
                table: "GameEngine",
                column: "GameId4");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId5",
                table: "GameEngine",
                column: "GameId5");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId6",
                table: "GameEngine",
                column: "GameId6");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CoverId",
                table: "Games",
                column: "CoverId");

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedCompany_CompanyId",
                table: "InvolvedCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedCompany_GameId",
                table: "InvolvedCompany",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDate_GameId",
                table: "ReleaseDate",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDate_PlatformId",
                table: "ReleaseDate",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarGame_CoverId",
                table: "SimilarGame",
                column: "CoverId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarGame_GameId",
                table: "SimilarGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Website_GameId",
                table: "Website",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgeRating_Games_GameId",
                table: "AgeRating",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cover_Games_GameId",
                table: "Cover",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cover_Games_GameId1",
                table: "Cover",
                column: "GameId1",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cover_Games_GameId",
                table: "Cover");

            migrationBuilder.DropForeignKey(
                name: "FK_Cover_Games_GameId1",
                table: "Cover");

            migrationBuilder.DropTable(
                name: "AgeRating");

            migrationBuilder.DropTable(
                name: "InvolvedCompany");

            migrationBuilder.DropTable(
                name: "ReleaseDate");

            migrationBuilder.DropTable(
                name: "SimilarGame");

            migrationBuilder.DropTable(
                name: "Website");

            migrationBuilder.DropTable(
                name: "GameEngine");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Cover");
        }
    }
}
