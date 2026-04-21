using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace School_Blog_project.Data.Migrations
{
	/// <inheritdoc />
	public partial class FixSeedDates : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			_ = migrationBuilder.AddColumn<bool>(
				name: "IsEditor",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			_ = migrationBuilder.AddColumn<bool>(
				name: "IsWriter",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			_ = migrationBuilder.CreateTable(
				name: "Articles",
				columns: table => new
				{
					ArticleID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DatePublished = table.Column<DateTime>(type: "datetime2", nullable: false),
					ArticleImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_Articles", x => x.ArticleID);
				});

			_ = migrationBuilder.CreateTable(
				name: "Readers",
				columns: table => new
				{
					UserID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
					IsWriter = table.Column<bool>(type: "bit", nullable: false),
					IsEditor = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_Readers", x => x.UserID);
				});

			_ = migrationBuilder.InsertData(
				table: "Articles",
				columns: ["ArticleID", "ArticleImagePath", "Author", "DatePublished", "Title"],
				values: new object[,]
				{
					{ 1, null, "dev_alice", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), "DEV: Welcome to the School Blog" },
					{ 2, null, "dev_bob", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), "DEV: Editorial Guidelines" },
					{ 3, null, "dev_alice", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), "DEV: Student Spotlight" },
					{ 4, null, "test_writer", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), "DEV: Test Article - Writer" },
					{ 5, null, "test_editor", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), "DEV: Test Article - Editor" }
				});

			_ = migrationBuilder.InsertData(
				table: "Readers",
				columns: ["UserID", "IsEditor", "IsWriter", "Password", "Username"],
				values: new object[,]
				{
					{ 1, false, true, "dev_pass_1", "dev_alice" },
					{ 2, true, false, "dev_pass_2", "dev_bob" },
					{ 3, false, true, "dev_pass_3", "dev_carol" },
					{ 4, false, true, "dev_pass_4", "test_writer" },
					{ 5, true, false, "dev_pass_5", "test_editor" },
					{ 6, false, true, "dev_pass_6", "test_both" }
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			_ = migrationBuilder.DropTable(
				name: "Articles");

			_ = migrationBuilder.DropTable(
				name: "Readers");

			_ = migrationBuilder.DropColumn(
				name: "IsEditor",
				table: "AspNetUsers");

			_ = migrationBuilder.DropColumn(
				name: "IsWriter",
				table: "AspNetUsers");
		}
	}
}

