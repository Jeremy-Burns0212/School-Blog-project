using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolBlogProject.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			_ = migrationBuilder.CreateTable(
				name: "Articles",
				columns: table => new
				{
					ArticleID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DatePublished = table.Column<DateTime>(type: "datetime2", nullable: false),
					IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
					ArticleImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_Articles", x => x.ArticleID);
				});

			_ = migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_AspNetRoles", x => x.Id);
				});

			_ = migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
					AccessFailedCount = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				});

			_ = migrationBuilder.CreateTable(
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
					_ = table.PrimaryKey("PK_Categories", x => x.CatagoryId);
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

			_ = migrationBuilder.CreateTable(
				name: "SiteSettings",
				columns: table => new
				{
					SiteSettingsId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					StartYear = table.Column<int>(type: "int", nullable: false),
					SchoolName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
					SchoolAcronym = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
					SchoolBlurb = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					SchoolLogo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					SchoolEmblem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_SiteSettings", x => x.SiteSettingsId);
				});

			_ = migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					_ = table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					_ = table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
					_ = table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					_ = table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					_ = table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					_ = table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
				name: "ArticleCategories",
				columns: table => new
				{
					ArticleID = table.Column<int>(type: "int", nullable: false),
					CatagoryId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_ArticleCategories", x => new { x.ArticleID, x.CatagoryId });
					_ = table.ForeignKey(
						name: "FK_ArticleCategories_Articles_ArticleID",
						column: x => x.ArticleID,
						principalTable: "Articles",
						principalColumn: "ArticleID",
						onDelete: ReferentialAction.Cascade);
					_ = table.ForeignKey(
						name: "FK_ArticleCategories_Categories_CatagoryId",
						column: x => x.CatagoryId,
						principalTable: "Categories",
						principalColumn: "CatagoryId",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
				name: "ColorSchemes",
				columns: table => new
				{
					SiteSettingsId = table.Column<int>(type: "int", nullable: false),
					Color1 = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
					Color2 = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
				},
				constraints: table =>
				{
					_ = table.PrimaryKey("PK_ColorSchemes", x => x.SiteSettingsId);
					_ = table.ForeignKey(
						name: "FK_ColorSchemes_SiteSettings_SiteSettingsId",
						column: x => x.SiteSettingsId,
						principalTable: "SiteSettings",
						principalColumn: "SiteSettingsId",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
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
					_ = table.PrimaryKey("PK_MediaContacts", x => x.SiteSettingsId);
					_ = table.ForeignKey(
						name: "FK_MediaContacts_SiteSettings_SiteSettingsId",
						column: x => x.SiteSettingsId,
						principalTable: "SiteSettings",
						principalColumn: "SiteSettingsId",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.CreateTable(
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
					_ = table.PrimaryKey("PK_OffSiteLinks", x => x.Id);
					_ = table.ForeignKey(
						name: "FK_OffSiteLinks_SiteSettings_SiteSettingsId",
						column: x => x.SiteSettingsId,
						principalTable: "SiteSettings",
						principalColumn: "SiteSettingsId",
						onDelete: ReferentialAction.Cascade);
				});

			_ = migrationBuilder.InsertData(
				table: "Articles",
				columns: ["ArticleID", "ArticleImagePath", "Author", "DatePublished", "Description", "Title"],
				values: new object[,]
				{
					{ 1, null, "dev_alice", new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc), null, "DEV: Welcome to the School Blog" },
					{ 2, null, "dev_bob", new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc), null, "DEV: Editorial Guidelines" },
					{ 3, null, "dev_alice", new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc), null, "DEV: Student Spotlight" },
					{ 4, null, "test_writer", new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc), null, "DEV: Test Article - Writer" },
					{ 5, null, "test_editor", new DateTime(2026, 4, 27, 5, 52, 52, 0, DateTimeKind.Utc), null, "DEV: Test Article - Editor" }
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

			_ = migrationBuilder.CreateIndex(
				name: "IX_ArticleCategories_CatagoryId",
				table: "ArticleCategories",
				column: "CatagoryId");

			_ = migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId");

			_ = migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			_ = migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId");

			_ = migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId");

			_ = migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId");

			_ = migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail");

			_ = migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			_ = migrationBuilder.CreateIndex(
				name: "IX_OffSiteLinks_SiteSettingsId",
				table: "OffSiteLinks",
				column: "SiteSettingsId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			_ = migrationBuilder.DropTable(
				name: "ArticleCategories");

			_ = migrationBuilder.DropTable(
				name: "AspNetRoleClaims");

			_ = migrationBuilder.DropTable(
				name: "AspNetUserClaims");

			_ = migrationBuilder.DropTable(
				name: "AspNetUserLogins");

			_ = migrationBuilder.DropTable(
				name: "AspNetUserRoles");

			_ = migrationBuilder.DropTable(
				name: "AspNetUserTokens");

			_ = migrationBuilder.DropTable(
				name: "ColorSchemes");

			_ = migrationBuilder.DropTable(
				name: "MediaContacts");

			_ = migrationBuilder.DropTable(
				name: "OffSiteLinks");

			_ = migrationBuilder.DropTable(
				name: "Readers");

			_ = migrationBuilder.DropTable(
				name: "Articles");

			_ = migrationBuilder.DropTable(
				name: "Categories");

			_ = migrationBuilder.DropTable(
				name: "AspNetRoles");

			_ = migrationBuilder.DropTable(
				name: "AspNetUsers");

			_ = migrationBuilder.DropTable(
				name: "SiteSettings");
		}
	}
}
