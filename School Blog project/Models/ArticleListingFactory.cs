using Microsoft.EntityFrameworkCore;
using School_Blog_project.Data;
using System.Text.RegularExpressions;

namespace School_Blog_project.Models
{
	/// <summary>
	/// Provides factory methods for creating view models representing paginated article listings, such as category and news
	/// pages.
	/// </summary>
	/// <remarks>This static class centralizes logic for constructing article listing pages, including support for
	/// filtering by category or year, pagination, and placeholder content for incomplete pages. Use these methods to
	/// generate view models suitable for display in UI components that present lists of articles.</remarks>
	public static class ArticleListingFactory
	{
		/// <summary>
		/// Default number of articles to display per page if not specified.
		/// </summary>
		private const int DefaultPageSize = 6;

		/// <summary>
		/// Represents the default placeholder text used for article titles.
		/// </summary>
		private const string PlaceholderTitle = "Article Title";

		/// <summary>
		/// Represents the URL of the default placeholder image used when no thumbnail image is available.
		/// </summary>
		private const string PlaceholderImageUrl = AssetPaths.PlaceholderImage;

		/// <summary>
		/// Asynchronously creates a view model for an article listing page filtered by one or more category aliases.
		/// </summary>
		/// <param name="heading">The heading text to display on the category page.</param>
		/// <param name="actionName">The name of the action used for generating pagination or navigation links.</param>
		/// <param name="page">The page number of results to retrieve. Must be greater than or equal to 1.</param>
		/// <param name="pageSize">The maximum number of articles to include on each page. Must be greater than 0.</param>
		/// <param name="year">Optional year filter to apply to the listing.</param>
		/// <param name="categoryAliases">An array of category aliases used to filter the articles displayed on the page.</param>
		public static Task<ArticleListingPageViewModel> CreateCategoryPageAsync(
			ApplicationDbContext context,
			string heading,
			string actionName,
			int page = 1,
			int pageSize = DefaultPageSize,
			int? year = null,
			params string[] categoryAliases)
		{
			string displayHeading = BuildHeading(heading, year);
			return CreatePageAsync(context, displayHeading, actionName, page, pageSize, year, categoryAliases);
		}

		/// <summary>
		/// Asynchronously creates a view model for a paginated news article listing, optionally filtered by year.
		/// </summary>
		public static Task<ArticleListingPageViewModel> CreateNewsPageAsync(
			ApplicationDbContext context,
			int page = 1,
			int pageSize = DefaultPageSize,
			int? year = null)
		{
			string heading = BuildHeading("News", year);
			return CreatePageAsync(context, heading, "News", page, pageSize, year);
		}

		/// <summary>
		/// Builds the visible heading for a listing page, appending the year when a filter is active.
		/// </summary>
		private static string BuildHeading(string heading, int? year)
		{
			return year.HasValue ? $"{heading} - {year.Value}" : heading;
		}

		/// <summary>
		/// Asynchronously creates an article listing page view model with articles filtered and paginated according to the
		/// specified criteria.
		/// </summary>
		/// <remarks>If no articles match the specified categories, the result will contain an empty article list. The
		/// method ensures the returned page size is at least 1 and fills the article list with placeholders if there are
		/// fewer articles than the requested page size.</remarks>
		private static async Task<ArticleListingPageViewModel> CreatePageAsync(
			ApplicationDbContext context,
			string heading,
			string actionName,
			int page,
			int pageSize,
			int? year,
			params string[] categoryAliases)
		{
			pageSize = Math.Max(1, pageSize);

			IQueryable<Article> query = context.Articles.AsNoTracking();

			// If a year filter is specified, calculate the start and end dates for that year and filter articles accordingly.
			if (year.HasValue)
			{
				DateTime startOfYear = new(year.Value, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				DateTime startOfNextYear = startOfYear.AddYears(1);

				query = query.Where(article =>
					article.DatePublished >= startOfYear &&
					article.DatePublished < startOfNextYear);
			}

			// If category aliases are provided, look up the corresponding category IDs and filter articles to those categories.
			if (categoryAliases.Length > 0)
			{
				List<int> categoryIds = await context.Categories
					.AsNoTracking()
					.Where(category =>
						categoryAliases.Contains(category.FullTitle) ||
						categoryAliases.Contains(category.ShortTitle))
					.Select(category => category.CatagoryId)
					.ToListAsync();

				// If we found matching category IDs, filter articles to those categories.
				// Otherwise, return an empty result.
				if (categoryIds.Count > 0)
				{
					query = query.Where(article =>
						article.ArticleCategories.Any(articleCategory =>
							categoryIds.Contains(articleCategory.CatagoryId)));
				}
				else
				{
					query = query.Where(_ => false);
				}
			}

			// Order articles by most recent publication date first.
			query = query.OrderByDescending(article => article.DatePublished);

			int totalCount = await query.CountAsync();
			int totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
			int currentPage = Math.Clamp(page, 1, totalPages);

			List<Article> pageArticles = await query
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			// Convert the retrieved articles to their summary representations for display in the listing.
			List<ArticleSummary> articles = pageArticles.Select(ToSummary).ToList();

			// If there are fewer articles than the requested page size, fill the remaining slots with placeholder summaries.
			while (articles.Count < pageSize)
			{
				articles.Add(CreatePlaceholder());
			}

			// Retrieve the list of available news archive years and the currently selected year for display in the sidebar or navigation.
			NewsArchivesViewModel archives = await NewsArchivesFactory.CreateAsync(context, year);

			return new ArticleListingPageViewModel
			{
				Heading = heading,
				ActionName = actionName,
				Articles = articles,
				CurrentPage = currentPage,
				TotalPages = totalPages,
				PageSize = pageSize,
				RouteValues = year.HasValue
					? new Dictionary<string, string> { ["year"] = year.Value.ToString() }
					: new Dictionary<string, string>(),
				NewsArchives = archives
			};
		}

		/// <summary>
		/// Creates a summary representation of the specified article for display or listing purposes.
		/// </summary>
		private static ArticleSummary ToSummary(Article article)
		{
			return new ArticleSummary
			{
				// Map the article's title, publication date, description, and image path to the summary.
				// Then generate a URL-friendly slug from the title for linking to the full article.
				// Then set IsPlaceholder to false since this is a real article summary,
				// and initialize the Categories list as empty (it can be populated later if needed).
				Title = article.Title,
				Date = article.DatePublished,
				Description = article.Description,
				ImageUrl = string.IsNullOrWhiteSpace(article.ArticleImagePath)
					? PlaceholderImageUrl
					: article.ArticleImagePath,
				Slug = Slugify(article.Title),
				IsPlaceholder = false,
				Categories = []
			};
		}

		/// <summary>
		/// Creates a placeholder instance of the ArticleSummary class with default or placeholder values.
		/// </summary>
		/// <remarks>Use this method to generate a non-null ArticleSummary object when actual article data is
		/// unavailable. This can be useful for UI scenarios where a placeholder is needed while loading or when no data
		/// exists.</remarks>
		private static ArticleSummary CreatePlaceholder()
		{
			return new ArticleSummary
			{
				Title = PlaceholderTitle,
				Date = default,
				Description = null,
				ImageUrl = PlaceholderImageUrl,
				Slug = string.Empty,
				IsPlaceholder = true,
				Categories = []
			};
		}

		/// <summary>
		/// Converts the specified string to a URL-friendly slug by lowercasing and replacing non-alphanumeric characters with
		/// hyphens.
		/// </summary>
		private static string Slugify(string value)
		{
			string slug = Regex.Replace(value.Trim().ToLowerInvariant(), @"[^a-z0-9]+", "-");
			return slug.Trim('-');
		}
	}
}