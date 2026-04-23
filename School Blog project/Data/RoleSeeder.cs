using Microsoft.AspNetCore.Identity;

namespace School_Blog_project.Data
{
	/// <summary>
	/// Ensures application roles exist. This runs at startup to create the minimal set
	/// of roles (Reader, Writer, Editor). Admin account provisioning remains in
	/// <see cref="AdminSeeder"/> so deployments can manage initial users separately.
	/// </summary>
	public class RoleSeeder(RoleManager<IdentityRole> roleManager)
	{
		private readonly RoleManager<IdentityRole> _roleManager = roleManager;

		public async Task SeedAsync(params string[] roles)
		{
			foreach (string role in roles)
			{
				if (!await _roleManager.RoleExistsAsync(role))
				{
					_ = await _roleManager.CreateAsync(new IdentityRole(role));
				}
			}
		}
	}
}
