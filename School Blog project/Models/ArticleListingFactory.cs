using Microsoft.EntityFrameworkCore;
using School_Blog_project.Data;
using System.Text.RegularExpressions;

namespace School_Blog_project.Models
{
	/// <summary>
	/// Shared article listing logic for category pages and the news page.
	/// </summary>
	public static class ArticleListingFactory
	{
		// Default number of articles to show per page. This is used when the caller doesn't specify a page size.
		private const int DefaultPageSize = 6;
		// Placeholder values for padding article lists when there are not enough articles to fill a page.
		private const string PlaceholderTitle = "Article Title";
		// A generic placeholder image URL to use when an article doesn't have an associated image.
		private const string PlaceholderImageUrl = AssetPaths.PlaceholderImage;

		/// <summary>
		/// Asynchronously creates a view model for a paginated article listing filtered by one or more category aliases.
		/// </summary>
		public static Task<ArticleListingPageViewModel> CreateCategoryPageAsync(
			ApplicationDbContext context,
			string heading,
			string actionName,
			int page = 1,
			int pageSize = DefaultPageSize,
			params string[] categoryAliases)
		{
			return CreatePageAsync(context, heading, actionName, page, pageSize, categoryAliases);
		}

		/// <summary>
		/// Creates an asynchronous view model for a paginated list of news articles.
		/// </summary>
		public static Task<ArticleListingPageViewModel> CreateNewsPageAsync(
			ApplicationDbContext context,
			int page = 1,
			int pageSize = DefaultPageSize)
		{
			return CreatePageAsync(context, "News", "News", page, pageSize);
		}

		/// <summary>
		/// Creates an article listing page view model for the specified page, page size, and optional category filters.
		/// </summary>
		/// <remarks>If no articles match the specified categories, the returned view model will contain placeholders
		/// to fill the requested page size. The method performs all database operations asynchronously.</remarks>
		private static async Task<ArticleListingPageViewModel> CreatePageAsync(
			ApplicationDbContext context,
			string heading,
			string actionName,
			int page,
			int pageSize,
			params string[] categoryAliases)
		{
			// Ensure page and pageSize are within reasonable bounds to prevent invalid queries.
			pageSize = Math.Max(1, pageSize);

			// Start with a base query for articles, using AsNoTracking for read-only performance.
			IQueryable<Article> query = context.Articles.AsNoTracking();

			// Filter by category only when aliases were supplied.
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

			// Retrieve only the articles for the current page using Skip and Take for pagination.
			List<Article> pageArticles = await query
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			// Convert the retrieved articles to summaries for the view model.
			List<ArticleSummary> articles = pageArticles
				.Select(ToSummary)
				.ToList();

			// If there are not enough articles to fill the page, add placeholders until we reach the
			// desired page size.
			while (articles.Count < pageSize)
			{
				articles.Add(CreatePlaceholder());
			}

			// Construct and return the view model with the retrieved articles and pagination info.
			return new ArticleListingPageViewModel
			{
				Heading = heading,
				ActionName = actionName,
				Articles = articles,
				CurrentPage = currentPage,
				TotalPages = totalPages,
				PageSize = pageSize
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