using Microsoft.AspNetCore.Identity;

namespace School_Blog_project.Data
{
	/// <summary>
	/// Helper to provision initial roles and admin users for a deployment.
	/// This file is intentionally not invoked automatically from Program.cs; teams can
	/// This file is intentionally not invoked automatically from Program.cs; teams can
	/// edit the static <see cref="SeedAsync"/> method to declare which accounts/roles
	/// should be created for their environment and then run it from a deployment
	/// script or a one-off admin tool.
	/// </summary>
	public static class AdminSeeder
	{
		/// <summary>
		/// Seed roles and users. The implementation below is an example: it creates
		/// the roles Reader, Writer and Editor and ensures a sample guest account exists.
		/// Edit the <see cref="initialAccounts"/> array to declare the accounts you want
		/// provisioned for a given deployment.
		/// </summary>
		public static async Task SeedAsync(IServiceProvider services)
		{
			using IServiceScope scope = services.CreateScope();
			RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// Roles to ensure exist (remove 'Reader' if not used)
			string[] roles = ["Writer", "Editor", "User"];
			foreach (string? role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					_ = await roleManager.CreateAsync(new IdentityRole(role));
				}
			}

			// Define initial accounts to create. Edit as needed for your deployment.
			var initialAccounts = new[]
			{
				new
				{
					Email = "Guest@Guest.com",
					Password = "P@ssw0rd",
					Roles = new[] { "User", "Writer", "Editor" }
				}
			};

			foreach (var acct in initialAccounts)
			{
				ApplicationUser? existing = await userManager.FindByEmailAsync(acct.Email);
				if (existing == null)
				{
					ApplicationUser user = new() { UserName = acct.Email, Email = acct.Email, EmailConfirmed = true };
					IdentityResult createResult = await userManager.CreateAsync(user, acct.Password);
					if (createResult.Succeeded)
					{
						_ = await userManager.AddToRolesAsync(user, acct.Roles);
					}
				}
				else
				{
					// ensure roles
					foreach (string? r in acct.Roles)
					{
						if (!await userManager.IsInRoleAsync(existing, r))
						{
							_ = await userManager.AddToRoleAsync(existing, r);
						}
					}
				}
			}
		}
	}
}
