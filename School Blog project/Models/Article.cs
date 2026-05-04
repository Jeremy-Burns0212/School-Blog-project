using System.ComponentModel.DataAnnotations;
namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents a blog article stored in the application database.
	/// </summary>
	public class Article
	{
		/// <summary>
		/// Primary key for the article.
		/// </summary>
		public int ArticleID { get; set; }

		/// <summary>
		/// The title of the article. This property is required.
		/// </summary>
		public required string Title { get; set; }

		/// <summary>
		/// The username of the author who created the article, if available.
		/// </summary>
		public string? Author { get; set; }

		/// <summary>
		/// UTC date and time the article was published.
		/// </summary>
		public DateTime DatePublished { get; set; }

		/// <summary>
		/// Indicates whether the article is featured. Stored as a SQL bit/boolean.
		/// Default value is <c>false</c>.
		/// </summary>
		public bool IsFeatured { get; set; }

		/// <summary>
		/// Optional path or URL to an image associated with the article.
		/// </summary>
		public string? ArticleImagePath { get; set; }

		/// <summary>
		/// Optional short summary/excerpt for preview text. Max length 120.
		/// </summary>
		[MaxLength(120)]
		public string? Description { get; set; }

		// Navigation collection for many-to-many linking to categories
		public ICollection<ArticleCategory> ArticleCategories { get; set; } = [];
	}
}
