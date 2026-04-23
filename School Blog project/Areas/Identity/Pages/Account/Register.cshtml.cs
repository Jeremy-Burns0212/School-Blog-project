using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Blog_project.Data;
using System.ComponentModel.DataAnnotations;

namespace School_Blog_project.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel(
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		RoleManager<IdentityRole> roleManager,
		ILogger<RegisterModel> logger) : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly RoleManager<IdentityRole> _roleManager = roleManager;
		private readonly ILogger<RegisterModel> _logger = logger;

		[BindProperty]
		public InputModel Input { get; set; } = null!;

		public string? ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; } = null!;

			[Required]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; } = null!;

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; } = null!;
		}

		public void OnGet(string? returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			if (ModelState.IsValid)
			{
				ApplicationUser user = new() { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
				IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
				if (result.Succeeded)
				{
#pragma warning disable CA2254 // Template should be a static expression
#pragma warning disable CA1848 // Use the LoggerMessage delegates
					_logger.LogInformation("User created a new account with password.");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
#pragma warning restore CA2254

					// Ensure the default Reader role exists and assign it to the new user
					const string defaultRole = "Reader";

					// Create the role if it doesn't exist
					if (!await _roleManager.RoleExistsAsync(defaultRole))
					{
						IdentityResult roleCreateResult = await _roleManager.CreateAsync(new IdentityRole(defaultRole));
						if (!roleCreateResult.Succeeded)
						{
							// Add role creation errors to ModelState
							foreach (IdentityError error in roleCreateResult.Errors)
							{
								ModelState.AddModelError(string.Empty, error.Description);
							}
							return Page();
						}
					}

					// Assign the role to the user
					if (!await _userManager.IsInRoleAsync(user, defaultRole))
					{
						IdentityResult roleResult = await _userManager.AddToRoleAsync(user, defaultRole);
						if (!roleResult.Succeeded)
						{
							// Add role assignment errors to ModelState
							foreach (IdentityError error in roleResult.Errors)
							{
								ModelState.AddModelError(string.Empty, error.Description);
							}
							return Page();
						}
					}

					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}
				// Add UserManager.CreateAsync errors to ModelState
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
