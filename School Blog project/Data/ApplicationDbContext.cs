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

		public DbSet<SiteSettings> SiteSettings { get; set; } = null!;
		public DbSet<MediaContact> MediaContacts { get; set; } = null!;
		public DbSet<ColorScheme> ColorSchemes { get; set; } = null!;
		public DbSet<OffSiteLink> OffSiteLinks { get; set; } = null!;

		public DbSet<ArticleCatagory> ArticleCatagories { get; set; } = null!;
		public DbSet<Categories> Categories { get; set; } = null!;

		/// <summary>
		/// Configures the EF Core model and seeds initial data used for development.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// SiteSettings relationships
			_ = builder.Entity<SiteSettings>()
				.HasOne(s => s.MediaContact)
				.WithOne(mc => mc.SiteSettings)
				.HasForeignKey<MediaContact>(mc => mc.SiteSettingsId)
				.OnDelete(DeleteBehavior.Cascade);

			_ = builder.Entity<SiteSettings>()
				.HasOne(s => s.ColorScheme)
				.WithOne(cs => cs.SiteSettings)
				.HasForeignKey<ColorScheme>(cs => cs.SiteSettingsId)
				.OnDelete(DeleteBehavior.Cascade);

			_ = builder.Entity<SiteSettings>()
				.HasMany(s => s.OffSiteLinks)
				.WithOne(l => l.SiteSettings)
				.HasForeignKey(l => l.SiteSettingsId)
				.OnDelete(DeleteBehavior.Cascade);

			// Configure the join entity ArticleCatagory: composite key and foreign keys
			_ = builder.Entity<ArticleCatagory>(eb =>
			{
				_ = eb.HasKey(ac => new { ac.ArticleID, ac.CatagoryId });

				_ = eb.HasOne(ac => ac.Article)
				  .WithMany(a => a.ArticleCatagories)
				  .HasForeignKey(ac => ac.ArticleID)
				  .OnDelete(DeleteBehavior.Cascade);

				_ = eb.HasOne(ac => ac.Catagory)
				  .WithMany(c => c.ArticleCatagories)
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
		}
	}
}
