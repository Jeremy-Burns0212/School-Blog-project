namespace School_Blog_project.Models
{
	/// <summary>
	/// Lightweight view model representing a summary of an article for lists.
	/// </summary>
	public class ArticleSummary
	{
		/// <summary>
		/// The article title.
		/// </summary>
		public required string Title { get; set; }

		/// <summary>
		/// Date displayed for the summary.
		/// </summary>
		public required DateTime Date { get; set; }

		/// <summary>
		/// Short description or excerpt for the article.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Optional URL for the image shown in the summary.
		/// </summary>
		public string? ImageUrl { get; set; }

		// Support multiple categories
		public List<string> Categories { get; set; } = new();
	}
}
