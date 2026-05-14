using Microsoft.AspNetCore.Routing;

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

		/// <summary>
		/// The route values used for generating pagination links. This may include parameters such as 
		/// category aliases or year filters, depending on the context of the article listing page. 
		/// For example, a category page might include a "category" parameter with the relevant alias, 
		/// while a news page might include a "year" parameter to filter articles by publication year. 
		/// These route values ensure that pagination links maintain the correct filtering context when 
		/// navigating between pages of articles.
		/// </summary>
		public IDictionary<string, string> RouteValues { get; init; } = new Dictionary<string, string>();

		/// <summary>
		/// The news archives view model, which contains the list of years for which news articles are 
		/// available and the currently selected year. This is used to display a sidebar or filter options 
		/// for news articles by year.
		/// </summary>
		public NewsArchivesViewModel NewsArchives { get; init; } = new();
	}
}