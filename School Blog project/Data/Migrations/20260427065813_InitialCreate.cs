using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Blog_project.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditor",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsWriter",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CatagoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortTitle = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FullTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CatagoryId);
                });

            migrationBuilder.CreateTable(
                name: "ArticleCatagories",
                columns: table => new
                {
                    ArticleID = table.Column<int>(type: "int", nullable: false),
                    CatagoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCatagories", x => new { x.ArticleID, x.CatagoryId });
                    table.ForeignKey(
                        name: "FK_ArticleCatagories_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleCatagories_Categories_CatagoryId",
                        column: x => x.CatagoryId,
                        principalTable: "Categories",
                        principalColumn: "CatagoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 1,
                column: "DatePublished",
                value: new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 2,
                column: "DatePublished",
                value: new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 3,
                column: "DatePublished",
                value: new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 4,
                column: "DatePublished",
                value: new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 5,
                column: "DatePublished",
                value: new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCatagories_CatagoryId",
                table: "ArticleCatagories",
                column: "CatagoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleCatagories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Articles");

            migrationBuilder.AddColumn<bool>(
                name: "IsEditor",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWriter",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 1,
                column: "DatePublished",
                value: new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 2,
                column: "DatePublished",
                value: new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 3,
                column: "DatePublished",
                value: new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 4,
                column: "DatePublished",
                value: new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleID",
                keyValue: 5,
                column: "DatePublished",
                value: new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
