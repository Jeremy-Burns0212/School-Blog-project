using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace School_Blog_project.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteSettingsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Readers");

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    SiteSettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SchoolAcronym = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SchoolBlurb = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SchoolLogo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolEmblem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.SiteSettingsId);
                });

            migrationBuilder.CreateTable(
                name: "ColorSchemes",
                columns: table => new
                {
                    SiteSettingsId = table.Column<int>(type: "int", nullable: false),
                    Color1 = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Color2 = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorSchemes", x => x.SiteSettingsId);
                    table.ForeignKey(
                        name: "FK_ColorSchemes_SiteSettings_SiteSettingsId",
                        column: x => x.SiteSettingsId,
                        principalTable: "SiteSettings",
                        principalColumn: "SiteSettingsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaContacts",
                columns: table => new
                {
                    SiteSettingsId = table.Column<int>(type: "int", nullable: false),
                    JobPosition = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaContacts", x => x.SiteSettingsId);
                    table.ForeignKey(
                        name: "FK_MediaContacts_SiteSettings_SiteSettingsId",
                        column: x => x.SiteSettingsId,
                        principalTable: "SiteSettings",
                        principalColumn: "SiteSettingsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffSiteLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteSettingsId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    URL = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffSiteLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffSiteLinks_SiteSettings_SiteSettingsId",
                        column: x => x.SiteSettingsId,
                        principalTable: "SiteSettings",
                        principalColumn: "SiteSettingsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OffSiteLinks_SiteSettingsId",
                table: "OffSiteLinks",
                column: "SiteSettingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorSchemes");

            migrationBuilder.DropTable(
                name: "MediaContacts");

            migrationBuilder.DropTable(
                name: "OffSiteLinks");

            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEditor = table.Column<bool>(type: "bit", nullable: false),
                    IsWriter = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.UserID);
                });

            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "UserID", "IsEditor", "IsWriter", "Password", "Username" },
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
    }
}
