namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents a reusable article listing page for Students, Graduates, Faculty, and News.
	/// </summary>
	public sealed class ArticleListingPageViewModel
	{
		/// <summary>
		/// Displayed page heading.
		/// </summary>
		public required string Heading { get; init; }

		/// <summary>
		/// MVC action name used for pagination links.
		/// </summary>
		public required string ActionName { get; init; }

		/// <summary>
		/// Article cards for the page. This list is padded with placeholders when needed.
		/// </summary>
		public IReadOnlyList<ArticleSummary> Articles { get; init; } = [];

		/// <summary>
		/// Current page number.
		/// </summary>
		public int CurrentPage { get; init; } = 1;

		/// <summary>
		/// Total number of pages.
		/// </summary>
		public int TotalPages { get; init; } = 1;

		/// <summary>
		/// Number of cards shown per page.
		/// </summary>
		public int PageSize { get; init; } = 6;
	}
}