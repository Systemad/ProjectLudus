using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludus.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlphaChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    Animated = table.Column<bool>(type: "INTEGER", nullable: false),
                    Height = table.Column<long>(type: "INTEGER", nullable: false),
                    ImageId = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Width = table.Column<long>(type: "INTEGER", nullable: false),
                    Checksum = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RatingCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rating = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    SteamId = table.Column<string>(type: "TEXT", nullable: false),
                    AvatarImageId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogoId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Logo_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Logo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LudusUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Users_LudusUserId",
                        column: x => x.LudusUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgeRating",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<long>(type: "INTEGER", nullable: false),
                    Rating = table.Column<long>(type: "INTEGER", nullable: false),
                    Checksum = table.Column<string>(type: "TEXT", nullable: false),
                    OrganizationId = table.Column<long>(type: "INTEGER", nullable: false),
                    RatingCategoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgeRating_RatingCategory_RatingCategoryId",
                        column: x => x.RatingCategoryId,
                        principalTable: "RatingCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlternativeName",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    Game = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Checksum = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cover",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageId = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cover", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    CoverId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    FirstReleaseDate = table.Column<long>(type: "INTEGER", nullable: false),
                    Hypes = table.Column<long>(type: "INTEGER", nullable: false),
                    MultiplayerModes = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    RatingCount = table.Column<long>(type: "INTEGER", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    TotalRating = table.Column<double>(type: "REAL", nullable: false),
                    TotalRatingCount = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Checksum = table.Column<string>(type: "TEXT", nullable: false),
                    GameTypeId = table.Column<long>(type: "INTEGER", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Games_GameType_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEngine",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Companies = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Platforms = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Checksum = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEngine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEngine_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameLocalization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Region = table.Column<long>(type: "INTEGER", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLocalization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameLocalization_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameMode",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true),
                    GameId1 = table.Column<long>(type: "INTEGER", nullable: true),
                    GameId2 = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameMode_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameMode_Games_GameId1",
                        column: x => x.GameId1,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameMode_Games_GameId2",
                        column: x => x.GameId2,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvolvedCompany",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<long>(type: "INTEGER", nullable: false),
                    Developer = table.Column<bool>(type: "INTEGER", nullable: false),
                    Publisher = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvolvedCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvolvedCompany_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvolvedCompany_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Platform_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReleaseDate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<long>(type: "INTEGER", nullable: false),
                    Platform = table.Column<long>(type: "INTEGER", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseDate_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SimilarGame",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<long>(type: "INTEGER", nullable: false),
                    CoverId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
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
                name: "Theme",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Checksum = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Theme_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VideoId = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Video_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Website",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<long>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
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
                name: "LanguageSupport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageId = table.Column<long>(type: "INTEGER", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSupport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageSupport_GameMode_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "GameMode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageSupport_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgeRating_GameId",
                table: "AgeRating",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AgeRating_OrganizationId",
                table: "AgeRating",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AgeRating_RatingCategoryId",
                table: "AgeRating",
                column: "RatingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeName_GameId",
                table: "AlternativeName",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_GameId",
                table: "Collection",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_LogoId",
                table: "Company",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cover_GameId",
                table: "Cover",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEngine_GameId",
                table: "GameEngine",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLocalization_GameId",
                table: "GameLocalization",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMode_GameId",
                table: "GameMode",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameMode_GameId1",
                table: "GameMode",
                column: "GameId1");

            migrationBuilder.CreateIndex(
                name: "IX_GameMode_GameId2",
                table: "GameMode",
                column: "GameId2");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CoverId",
                table: "Games",
                column: "CoverId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameTypeId",
                table: "Games",
                column: "GameTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_LudusUserId",
                table: "Images",
                column: "LudusUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedCompany_CompanyId",
                table: "InvolvedCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedCompany_GameId",
                table: "InvolvedCompany",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSupport_GameId",
                table: "LanguageSupport",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSupport_LanguageId",
                table: "LanguageSupport",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Platform_GameId",
                table: "Platform",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDate_GameId",
                table: "ReleaseDate",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarGame_CoverId",
                table: "SimilarGame",
                column: "CoverId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarGame_GameId",
                table: "SimilarGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Theme_GameId",
                table: "Theme",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Video_GameId",
                table: "Video",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Website_GameId",
                table: "Website",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgeRating_GameMode_OrganizationId",
                table: "AgeRating",
                column: "OrganizationId",
                principalTable: "GameMode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AgeRating_Games_GameId",
                table: "AgeRating",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativeName_Games_GameId",
                table: "AlternativeName",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Collection_Games_GameId",
                table: "Collection",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cover_Games_GameId",
                table: "Cover",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cover_Games_GameId",
                table: "Cover");

            migrationBuilder.DropTable(
                name: "AgeRating");

            migrationBuilder.DropTable(
                name: "AlternativeName");

            migrationBuilder.DropTable(
                name: "Collection");

            migrationBuilder.DropTable(
                name: "GameEngine");

            migrationBuilder.DropTable(
                name: "GameLocalization");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "InvolvedCompany");

            migrationBuilder.DropTable(
                name: "LanguageSupport");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "ReleaseDate");

            migrationBuilder.DropTable(
                name: "SimilarGame");

            migrationBuilder.DropTable(
                name: "Theme");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Website");

            migrationBuilder.DropTable(
                name: "RatingCategory");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "GameMode");

            migrationBuilder.DropTable(
                name: "Logo");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Cover");

            migrationBuilder.DropTable(
                name: "GameType");
        }
    }
}
