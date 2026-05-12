using System;
using System.Collections.Generic;

namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents the data model for the home page, including featured and news stories, as well as pagination
	/// information.
	/// </summary>
	public sealed class HomePageViewModel
	{
		/// <summary>
		/// The main featured story to be prominently displayed on the home page. This is the most recent of the featured articles.
		/// </summary>
		public HomePageArticleViewModel? MainFeaturedStory { get; init; }

		/// <summary>
		/// Gets the collection of featured stories to display on the home page.
		/// </summary>
		public IReadOnlyList<HomePageArticleViewModel> FeaturedStories { get; init; } = [];

		/// <summary>
		/// Gets the collection of news stories to display on the home page.
		/// </summary>
		public IReadOnlyList<HomePageArticleViewModel> NewsStories { get; init; } = [];

		/// <summary>
		/// Gets the current page number in the news pagination sequence.
		/// </summary>
		public int CurrentNewsPage { get; init; } = 1;

		/// <summary>
		/// Gets the total number of pages available for news results.
		/// </summary>
		public int TotalNewsPages { get; init; } = 1;
	}

	/// <summary>
	/// Represents the data required to display an article on the home page, including its identifier, title, publication
	/// date, description, thumbnail image URL, and URL-friendly slug.
	/// </summary>
	public sealed class HomePageArticleViewModel
	{
		/// <summary>
		/// Gets the unique identifier for the article.
		/// </summary>
		public int ArticleID { get; init; }

		/// <summary>
		/// Gets the title associated with the article.
		/// </summary>
		public required string Title { get; init; }

		/// <summary>
		/// Gets the date and time when the article was published.
		/// </summary>
		public DateTime DatePublished { get; init; }
		
		/// <summary>
		/// Gets the description of the article.
		/// </summary>
		public string? Description { get; init; }

		/// <summary>
		/// Gets the URL of the thumbnail image associated with this article.
		/// </summary>
		public required string ImageUrl { get; init; }
		
		/// <summary>
		/// Gets the URL-friendly slug for the article.
		/// </summary>
		public required string Slug { get; init; }

		/// <summary>
		/// Indicates whether this item is a placeholder.
		/// </summary>
		public bool IsPlaceholder { get; init; }
	}
}
