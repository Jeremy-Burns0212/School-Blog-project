using Microsoft.AspNetCore.Identity;

namespace School_Blog_project.Data
{
	/// <summary>
	/// Extends IdentityUser with application specific flags for roles-like behavior.
	/// </summary>
	public class ApplicationUser : IdentityUser
	{
		// Role membership is used instead of boolean flags (Writer, Editor)
	}
}
