using Microsoft.AspNetCore.Identity;

namespace School_Blog_project.Data
{
	public class ApplicationUser : IdentityUser
	{
		public bool IsWriter { get; set; }

		public bool IsEditor { get; set; }
	}
}
