using Microsoft.AspNetCore.Identity;

namespace School_Blog_project.Data
{
	/// <summary>
	/// Extends IdentityUser with application specific flags for roles-like behavior.
	/// </summary>
	public class ApplicationUser : IdentityUser
	{
		/// <summary>
		/// Indicates whether the user is permitted to create articles.
		/// </summary>
		public bool IsWriter { get; set; }

		/// <summary>
		/// Indicates whether the user is permitted to edit or approve articles.
		/// </summary>
		public bool IsEditor { get; set; }
	}
}
