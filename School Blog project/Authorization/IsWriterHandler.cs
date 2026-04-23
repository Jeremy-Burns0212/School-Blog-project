using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using School_Blog_project.Data;

namespace School_Blog_project.Authorization
{
	public class IsWriterHandler(UserManager<ApplicationUser> userManager) : AuthorizationHandler<IsWriterRequirement>
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsWriterRequirement requirement)
		{
			if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
			{
				return;
			}

			ApplicationUser? user = await _userManager.GetUserAsync(context.User);
			if (user != null && await _userManager.IsInRoleAsync(user, "Writer"))
			{
				context.Succeed(requirement);
			}
		}
	}
}
