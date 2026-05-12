using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School_Blog_project.Models;

#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace School_Blog_project.Data
{
	/// <summary>
	/// EF Core database context for the School Blog application.
	/// Inherits from <see cref="IdentityDbContext{ApplicationUser}"/> to include identity schema.
	/// </summary>
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
	{
		/// <summary>
		/// DbSet for articles persisted in the application database.
		/// </summary>
		public DbSet<Article> Articles { get; set; } = null!;

		/// <summary>
		/// DbSet for reader seed records.
		/// </summary>
		public DbSet<Reader> Readers { get; set; } = null!;

		public DbSet<ArticleCategory> ArticleCategories { get; set; } = null!;
		public DbSet<Categories> Categories { get; set; } = null!;

		/// <summary>
		/// DbSet for site settings.
		/// </summary>
		public DbSet<SiteSettings> SiteSettings { get; set; } = null!;

		/// <summary>
		/// DbSet for media contact information.
		/// </summary>
		public DbSet<MediaContact> MediaContacts { get; set; } = null!;

		/// <summary>
		/// DbSet for color scheme configuration.
		/// </summary>
		public DbSet<ColorScheme> ColorSchemes { get; set; } = null!;

		/// <summary>
		/// DbSet for off-site links.
		/// </summary>
		public DbSet<OffSiteLink> OffSiteLinks { get; set; } = null!;

		/// <summary>
		/// Configures the EF Core model and seeds initial data used for development.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Seed Readers (matches Database/seed_dev.sql inserts and subsequent updates)
			_ = builder.Entity<Reader>().HasData(
				new Reader { UserID = 1, Username = "dev_alice", Password = "dev_pass_1", IsWriter = true, IsEditor = false },
				new Reader { UserID = 2, Username = "dev_bob", Password = "dev_pass_2", IsWriter = false, IsEditor = true },
				new Reader { UserID = 3, Username = "dev_carol", Password = "dev_pass_3", IsWriter = true, IsEditor = false }, // granted writer in SQL update
				new Reader { UserID = 4, Username = "test_writer", Password = "dev_pass_4", IsWriter = true, IsEditor = false },
				new Reader { UserID = 5, Username = "test_editor", Password = "dev_pass_5", IsWriter = false, IsEditor = true },
				new Reader { UserID = 6, Username = "test_both", Password = "dev_pass_6", IsWriter = true, IsEditor = false } // editor revoked in SQL update
			);

			// Configure the join entity ArticleCategory: composite key and foreign keys
			_ = builder.Entity<ArticleCategory>(eb =>
			{
				_ = eb.HasKey(ac => new { ac.ArticleID, ac.CatagoryId });

				_ = eb.HasOne(ac => ac.Article)
				  .WithMany(a => a.ArticleCategories)
				  .HasForeignKey(ac => ac.ArticleID)
				  .OnDelete(DeleteBehavior.Cascade);

				_ = eb.HasOne(ac => ac.Category)
				  .WithMany(c => c.ArticleCategories)
				  .HasForeignKey(ac => ac.CatagoryId)
				  .OnDelete(DeleteBehavior.Cascade);
			});

			// Seed Articles (uses fixed DatePublished values to mirror SYSUTCDATETIME at insert time)
			DateTime now = new(2026, 4, 27, 5, 52, 52, DateTimeKind.Utc);
			// Ensure database column default
			_ = builder.Entity<Article>().Property(a => a.IsFeatured).HasDefaultValue(false);

			_ = builder.Entity<Article>().HasData(
				new Article { ArticleID = 1, Title = "DEV: Welcome to the School Blog", Author = "dev_alice", DatePublished = now, ArticleImagePath = null, IsFeatured = false },
				new Article { ArticleID = 2, Title = "DEV: Editorial Guidelines", Author = "dev_bob", DatePublished = now, ArticleImagePath = null, IsFeatured = false },
				new Article { ArticleID = 3, Title = "DEV: Student Spotlight", Author = "dev_alice", DatePublished = now, ArticleImagePath = null, IsFeatured = false },
				new Article { ArticleID = 4, Title = "DEV: Test Article - Writer", Author = "test_writer", DatePublished = now, ArticleImagePath = null, IsFeatured = false },
				new Article { ArticleID = 5, Title = "DEV: Test Article - Editor", Author = "test_editor", DatePublished = now, ArticleImagePath = null, IsFeatured = false }
			);

			// Configure SiteSettings relationships
			// MediaContact: one-to-one with SiteSettings
			_ = builder.Entity<MediaContact>(eb =>
			{
				_ = eb.HasOne(mc => mc.SiteSettings)
					.WithOne(ss => ss.MediaContact)
					.HasForeignKey<MediaContact>(mc => mc.SiteSettingsId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// ColorScheme: one-to-one with SiteSettings
			_ = builder.Entity<ColorScheme>(eb =>
			{
				_ = eb.HasOne(cs => cs.SiteSettings)
					.WithOne(ss => ss.ColorScheme)
					.HasForeignKey<ColorScheme>(cs => cs.SiteSettingsId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// OffSiteLink: one-to-many with SiteSettings
			_ = builder.Entity<OffSiteLink>(eb =>
			{
				_ = eb.HasOne(osl => osl.SiteSettings)
					.WithMany(ss => ss.OffSiteLinks)
					.HasForeignKey(osl => osl.SiteSettingsId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
