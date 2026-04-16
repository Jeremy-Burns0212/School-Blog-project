using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School_Blog_project.Models;

#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace School_Blog_project.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
	{
		public DbSet<Article> Articles { get; set; } = null!;
		public DbSet<Reader> Readers { get; set; } = null!;

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

			// Seed Articles (uses fixed DatePublished values to mirror SYSUTCDATETIME at insert time)
			DateTime now = DateTime.UtcNow;
			_ = builder.Entity<Article>().HasData(
				new Article { ArticleID = 1, Title = "DEV: Welcome to the School Blog", Author = "dev_alice", DatePublished = now, ArticleImagePath = null },
				new Article { ArticleID = 2, Title = "DEV: Editorial Guidelines", Author = "dev_bob", DatePublished = now, ArticleImagePath = null },
				new Article { ArticleID = 3, Title = "DEV: Student Spotlight", Author = "dev_alice", DatePublished = now, ArticleImagePath = null },
				new Article { ArticleID = 4, Title = "DEV: Test Article - Writer", Author = "test_writer", DatePublished = now, ArticleImagePath = null },
				new Article { ArticleID = 5, Title = "DEV: Test Article - Editor", Author = "test_editor", DatePublished = now, ArticleImagePath = null }
			);
		}
	}
}
