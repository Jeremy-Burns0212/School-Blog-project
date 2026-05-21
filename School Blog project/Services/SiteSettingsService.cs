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
		public async Task<SiteSettings> GetCurrentAsync()
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