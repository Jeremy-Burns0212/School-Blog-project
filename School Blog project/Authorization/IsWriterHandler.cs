using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using School_Blog_project.Data;

namespace School_Blog_project.Authorization
{
	public class IsWriterHandler(UserManager<ApplicationUser> userManager) : AuthorizationHandler<IsWriterRequirement>
	{
		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsWriterRequirement requirement)
		{
			if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
			{
				return;
			}

			ApplicationUser? user = await userManager.GetUserAsync(context.User);
			if (user != null && user.IsWriter)
			{
				context.Succeed(requirement);
			}
		}
	}
}
