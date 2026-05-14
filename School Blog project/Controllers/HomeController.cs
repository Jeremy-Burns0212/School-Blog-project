using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Blog_project.Data;
using School_Blog_project.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace School_Blog_project.Controllers
{
	public class HomeController : Controller
	{
		private const int NewsPageSize = 6;
		private const string PlaceholderImageUrl = AssetPaths.PlaceholderImage;

		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Returns the home page view with featured stories and paginated news articles. It retrieves the most recent
		/// featured article to display as the main story and up to three additional featured articles for secondary display.
		/// The news section is paginated, showing a fixed number of articles per page, and calculates the total number of
		/// pages based on the total count of news articles in the database.
		/// </summary>
		public async Task<IActionResult> Index(int newsPage = 1)
		{
			// Retrieve all featured articles, ordered by publication date
			List<Article> featuredArticles = await _context.Articles
				.AsNoTracking()
				.Where(a => a.IsFeatured)
				.OrderByDescending(a => a.DatePublished)
				.ToListAsync();

			// The most recent featured article is designated as the main featured story.
			HomePageArticleViewModel mainFeaturedStory = featuredArticles
				.FirstOrDefault() is Article mainFeatured
					? ToViewModel(mainFeatured)
					: CreatePlaceholderArticle();

			// Skip the main featured story and take up to three additional featured stories for secondary display.
			List<HomePageArticleViewModel> additionalFeaturedStories = PadWithPlaceholders(
				featuredArticles
					.Skip(1)
					.Take(3)
					.Select(ToViewModel),
				3);

			// Prepare the query for news articles, ordered by publication date, and calculate pagination details.
			IQueryable<Article> newsQuery = _context.Articles
				.AsNoTracking()
				.OrderByDescending(a => a.DatePublished);

			int totalNewsCount = await newsQuery.CountAsync();
			int totalNewsPages = Math.Max(1, (int)Math.Ceiling(totalNewsCount / (double)NewsPageSize));
			int currentNewsPage = Math.Clamp(newsPage, 1, totalNewsPages);

			// Retrieve the news articles for the current page using Skip and Take for pagination.
			List<Article> recentNewsArticles = await newsQuery
				.Skip((currentNewsPage - 1) * NewsPageSize)
				.Take(NewsPageSize)
				.ToListAsync();

			// Convert the retrieved news articles to their corresponding view models for display on the home page.
			List<HomePageArticleViewModel> recentNewsStories = PadWithPlaceholders(
				recentNewsArticles.Select(ToViewModel),
				NewsPageSize);

			// Create the view model for the home page, populating it with the main featured story, additional featured stories,
			HomePageViewModel viewModel = new()
			{
				MainFeaturedStory = mainFeaturedStory,
				FeaturedStories = additionalFeaturedStories,
				NewsStories = recentNewsStories,
				CurrentNewsPage = currentNewsPage,
				TotalNewsPages = totalNewsPages
			};

			// Return the home page view, passing the populated view model to be rendered.
			return View(viewModel);
		}

		/// <summary>
		/// Handles HTTP GET requests for the Articles page, displaying a specific article by its title slug or the most
		/// recent article if no slug is provided.
		/// </summary>
		/// <remarks>If the specified article does not exist, the method returns a 404 Not Found response. The title
		/// slug is matched using a slugified version of the article title.</remarks>
		/// <param name="titleSlug">An optional URL-friendly string representing the article title. If null or empty, the most recently published
		/// article is displayed.</param>
		[HttpGet("Home/Articles/{titleSlug?}")]
		public async Task<IActionResult> Articles(string? titleSlug = null)
		{
			// Retrieve all articles from the database without tracking to improve performance, as we only need to read the data.
			List<Article> articles = await _context.Articles
				.AsNoTracking()
				.ToListAsync();

			// If a title slug is provided, attempt to find the article with a matching slugified title.
			// If no slug is provided, select the most recent article.
			Article? article = string.IsNullOrWhiteSpace(titleSlug)
				? articles.OrderByDescending(a => a.DatePublished).FirstOrDefault()
				: articles.FirstOrDefault(a => Slugify(a.Title) == titleSlug);

			// If no matching article is found, return a 404 Not Found response.
			if (article is null)
			{
				return NotFound();
			}

			// Convert the found article to its corresponding view model and return the Articles view to display it.
			return View(ToViewModel(article));
		}

		/// <summary>
		/// Returns the privacy page view.
		/// </summary>
		public IActionResult Privacy()
		{
			return View();
		}

		/// <summary>
		/// Returns the students information page view with paginated articles categorized under "Student Stories" or "Students".
		/// </summary>
		public async Task<IActionResult> Students(int page = 1, int? year = null)
		{
			return View(await ArticleListingFactory.CreateCategoryPageAsync(
				_context,
				heading: "Student Stories",
				actionName: nameof(Students),
				page: page,
				pageSize: 6,
				year: year,
				categoryAliases: ["Student Stories", "Students"]));
		}

		/// <summary>
		/// Returns the graduates information page view with paginated articles categorized under "Graduate Stories" or "Graduates".
		/// </summary>
		public async Task<IActionResult> Graduates(int page = 1, int? year = null)
		{
			return View(await ArticleListingFactory.CreateCategoryPageAsync(
				_context,
				heading: "Graduate Stories",
				actionName: nameof(Graduates),
				page: page,
				pageSize: 6,
				year: year,
				categoryAliases: ["Graduate Stories", "Graduates"]));
		}

		/// <summary>
		/// Returns the faculty information page view with paginated articles categorized under "Faculty & Staff Stories", "Faculty & Staff", or "Faculty".
		/// </summary>
		public async Task<IActionResult> Faculty(int page = 1, int? year = null)
		{
			return View(await ArticleListingFactory.CreateCategoryPageAsync(
				_context,
				heading: "Faculty & Staff Stories",
				actionName: nameof(Faculty),
				page: page,
				pageSize: 6,
				year: year,
				categoryAliases: ["Faculty & Staff Stories", "Faculty & Staff", "Faculty"]));
		}

		/// <summary>
		/// Returns the news listing view with paginated articles categorized under "News".
		/// </summary>
		public async Task<IActionResult> News(int page = 1, int? year = null)
		{
			return View(await ArticleListingFactory.CreateNewsPageAsync(
				_context,
				page: page,
				pageSize: 6,
				year: year));
		}

		/// <summary>
		/// Handles requests for the error page and returns a view that displays error details to the user.
		/// </summary>
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		/// <summary>
		/// Creates a new instance of HomePageArticleViewModel based on the specified Article entity.
		/// </summary>
		/// <remarks>If the ArticleImagePath property of the article is null, empty, or consists only of white-space
		/// characters, a placeholder image URL is used for the ImageUrl property.</remarks>
		private static HomePageArticleViewModel ToViewModel(Article article)
		{
			// Convert the Article entity to a HomePageArticleViewModel, mapping relevant properties and generating a slug for the title.
			return new HomePageArticleViewModel
			{
				// Map the ArticleID to ArticleID, Title to Title, and DatePublished to DatePublished in the view model.
				// The Description is set to the article description when available.
				// The ImageUrl is determined based on whether the ArticleImagePath is provided; if not, a placeholder image URL is used.
				// The Slug is generated by slugifying the article title to create a URL-friendly string.
				ArticleID = article.ArticleID,
				Title = article.Title,
				DatePublished = article.DatePublished,
				Description = article.Description,
				ImageUrl = string.IsNullOrWhiteSpace(article.ArticleImagePath)
					? PlaceholderImageUrl
					: article.ArticleImagePath,
				Slug = Slugify(article.Title),
				IsPlaceholder = false
			};
		}

		/// <summary>
		/// Creates a placeholder home page article model.
		/// </summary>
		private static HomePageArticleViewModel CreatePlaceholderArticle()
		{
			return new HomePageArticleViewModel
			{
				ArticleID = 0,
				Title = "Article Title",
				DatePublished = default,
				Description = null,
				ImageUrl = PlaceholderImageUrl,
				Slug = string.Empty,
				IsPlaceholder = true
			};
		}

		/// <summary>
		/// Pads the supplied list with placeholder items until it reaches the requested count.
		/// </summary>
		private static List<HomePageArticleViewModel> PadWithPlaceholders(IEnumerable<HomePageArticleViewModel> items, int count)
		{
			List<HomePageArticleViewModel> list = items.ToList();

			while (list.Count < count)
			{
				list.Add(CreatePlaceholderArticle());
			}

			return list;
		}

		// Converts a given string into a URL-friendly slug by removing non-alphanumeric characters and replacing them with hyphens.
		private static string Slugify(string value)
		{
			string slug = Regex.Replace(value.Trim().ToLowerInvariant(), @"[^a-z0-9]+", "-");
			return slug.Trim('-');
		}
	}
}
