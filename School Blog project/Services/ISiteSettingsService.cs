using SchoolBlogProject.Models;

namespace SchoolBlogProject.Services
{
	/// <summary>
	/// This interface defines the contract for a service that provides operations
	/// related to site settings.
	/// </summary>
	public interface ISiteSettingsService
	{
		Task<SiteSettings> GetCurrentAsync();
	}
}