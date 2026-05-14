namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents the information about an article that is displayed in the _ArticleList view across the 
	/// Students, Graduates, Faculty, and News pages.
	/// </summary>
	public class ArticleSummary
	{
		/// <summary>
		/// Stores the title of the article.
		/// </summary>
		public required string Title { get; set; }

		/// <summary>
		/// The date when the article was published. 
		/// This is necessary for sorting articles chronologically and displaying the publication date to users.
		/// </summary>
		public required DateTime Date { get; set; }

		/// <summary>
		/// A brief summary or excerpt of the article's content. 
		/// This is useful for giving users a quick overview of the article before they decide to read the full content.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Stores the URL of the thumbnail image associated with the article.
		/// </summary>
		public string? ImageUrl { get; set; }

		/// <summary>
		/// A URL-friendly slug for linking to the full article.
		/// </summary>
		public string Slug { get; set; } = string.Empty;

		/// <summary>
		/// Indicates whether this item is a placeholder.
		/// </summary>
		public bool IsPlaceholder { get; set; }

		/// <summary>
		/// A list of categories to which the article belongs.
		/// Needs to be a list because an article can belong to multiple categories.
		/// </summary>
		public List<string> Categories { get; set; } = new();
	}
}
