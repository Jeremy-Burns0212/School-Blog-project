using Microsoft.EntityFrameworkCore;
using School_Blog_project.Data;
using School_Blog_project.Models;

namespace School_Blog_project.Services
{
	/// <summary>
	/// Provides operations for retrieving and managing site settings from the application's data store.
	/// </summary>
	/// <remarks>This service is intended to be used as the primary means of accessing site-wide configuration and
	/// related entities, mainly used for the UI.</remarks>
	public class SiteSettingsService : ISiteSettingsService
	{
		private readonly ApplicationDbContext _context;

		// Cache the current settings task so the database is only queried once per request.
		private Task<SiteSettings>? _currentSiteSettingsTask;

		/// <summary>
		/// Initializes a new instance of the SiteSettingsService class using the specified database context.
		/// </summary>
		public SiteSettingsService(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Asynchronously retrieves the most recent site settings from the data store.
		/// </summary>
		/// <remarks>The returned <see cref="SiteSettings"/> instance includes related entities such as media contact,
		/// color scheme, and off-site links. The method does not track changes to the retrieved entities.</remarks>
		public Task<SiteSettings> GetCurrentAsync()
		{
			_currentSiteSettingsTask ??= LoadCurrentAsync();
			return _currentSiteSettingsTask;
		}

		/// <summary>
		/// Loads the current site settings from the database.
		/// </summary>
		private async Task<SiteSettings> LoadCurrentAsync()
		{
			SiteSettings? siteSettings = await _context.SiteSettings
				.AsNoTracking()
				.Include(settings => settings.MediaContact)
				.Include(settings => settings.ColorScheme)
				.Include(settings => settings.OffSiteLinks)
				.OrderByDescending(settings => settings.SiteSettingsId)
				.FirstOrDefaultAsync();

			return siteSettings ?? new SiteSettings();
		}
	}
}