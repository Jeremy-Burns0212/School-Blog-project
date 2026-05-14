using Microsoft.EntityFrameworkCore;
using School_Blog_project.Data;

namespace School_Blog_project.Models
{
	/// <summary>
	/// Provides a factory for creating instances of the NewsArchivesViewModel class based on available article publication
	/// years.
	/// </summary>
	/// <remarks>This static class is intended to centralize the logic for constructing NewsArchivesViewModel
	/// objects, ensuring consistent population of year ranges for news archives throughout the application.</remarks>
	public static class NewsArchivesFactory
	{
		/// <summary>
		/// Asynchronously creates a new instance of the NewsArchivesViewModel containing available publication years and the
		/// selected year.
		/// </summary>
		/// <remarks>If no articles exist in the database, the current year is used as the only available
		/// year.</remarks>
		public static async Task<NewsArchivesViewModel> CreateAsync(
			ApplicationDbContext context,
			int? selectedYear = null)
		{
			var yearRange = await context.Articles
				.AsNoTracking()
				.GroupBy(_ => 1)
				.Select(g => new
				{
					MinYear = g.Min(article => article.DatePublished.Year),
					MaxYear = g.Max(article => article.DatePublished.Year)
				})
				.FirstOrDefaultAsync();

			if (yearRange is null)
			{
				return new NewsArchivesViewModel
				{
					Years = [DateTime.UtcNow.Year],
					SelectedYear = selectedYear
				};
			}

			return new NewsArchivesViewModel
			{
				Years = Enumerable.Range(yearRange.MinYear, yearRange.MaxYear - yearRange.MinYear + 1).ToArray(),
				SelectedYear = selectedYear
			};
		}
	}
}