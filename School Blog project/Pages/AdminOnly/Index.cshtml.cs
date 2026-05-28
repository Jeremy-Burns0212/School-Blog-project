using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using School_Blog_project.Data;
using School_Blog_project.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace School_Blog_project.Pages.AdminOnly
{
	[Authorize(Roles = "Admin")]
	public class IndexModel : PageModel
	{
		private const string AdminRole = "Admin";
		private const string WriterRole = "Writer";
		private const string EditorRole = "Editor";
		private const string GeneralRole = "User";

		private readonly ApplicationDbContext _context;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;
		public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		[BindProperty]
		public AdminSettingsInputModel SettingsForm { get; set; } = new();

		[BindProperty]
		public OffSiteLinkInputModel NewLink { get; set; } = new();

		[BindProperty]
		public UserRoleInputModel RoleForm { get; set; } = new();

		public IReadOnlyList<UserRowViewModel> WriterUsers { get; private set; } = [];

		public IReadOnlyList<UserRowViewModel> EditorUsers { get; private set; } = [];

		public IReadOnlyList<UserRowViewModel> GeneralUsers { get; private set; } = [];

		public IReadOnlyList<UserRowViewModel> AdminUsers { get; private set; } = [];

		public bool IsCurrentUserSuperAdmin { get; private set; }

		public string? PrimaryAdminUserId { get; private set; }

		public IReadOnlyList<OffSiteLinkViewModel> OffSiteLinks { get; private set; } = [];

		public async Task OnGetAsync()
		{
			await LoadPageAsync();
		}

		public async Task<IActionResult> OnPostSaveSettingsAsync()
		{
			if (!ModelState.IsValid)
			{
				await LoadPageAsync();
				return Page();
			}

			SiteSettings settings = await EnsureSiteSettingsAsync();
			settings.StartYear = SettingsForm.StartYear;
			settings.SchoolName = SettingsForm.SchoolName.Trim();
			settings.SchoolAcronym = SettingsForm.SchoolAcronym.Trim();
			settings.SchoolBlurb = SettingsForm.SchoolBlurb.Trim();
			settings.SchoolLogo = SettingsForm.SchoolLogo.Trim();
			settings.SchoolEmblem = SettingsForm.SchoolEmblem.Trim();

			if (settings.MediaContact is null)
			{
				settings.MediaContact = new MediaContact();
			}

			settings.MediaContact.JobPosition = SettingsForm.JobPosition?.Trim();
			settings.MediaContact.FullName = SettingsForm.FullName?.Trim();
			settings.MediaContact.Phone = SettingsForm.Phone?.Trim();
			settings.MediaContact.Email = SettingsForm.Email?.Trim();

			if (settings.ColorScheme is null)
			{
				settings.ColorScheme = new ColorScheme();
			}

			settings.ColorScheme.Color1 = NormalizeHex(SettingsForm.PrimaryColor);
			settings.ColorScheme.Color2 = NormalizeHex(SettingsForm.SecondaryColor);

			_ = await _context.SaveChangesAsync();
			TempData["StatusMessage"] = "Site settings were updated successfully.";

			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostAddLinkAsync()
		{
			if (!ModelState.IsValid)
			{
				await LoadPageAsync();
				return Page();
			}

			SiteSettings settings = await EnsureSiteSettingsAsync();
			settings.OffSiteLinks.Add(new OffSiteLink
			{
				Name = NewLink.Name.Trim(),
				URL = NewLink.Url.Trim()
			});

			_ = await _context.SaveChangesAsync();
			TempData["StatusMessage"] = "Off-site link added.";

			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostDeleteLinkAsync(int offSiteLinkId)
		{
			OffSiteLink? link = await _context.OffSiteLinks.FirstOrDefaultAsync(item => item.Id == offSiteLinkId);
			if (link is not null)
			{
				_ = _context.OffSiteLinks.Remove(link);
				_ = await _context.SaveChangesAsync();
				TempData["StatusMessage"] = "Off-site link removed.";
			}

			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostUpdateUserRoleAsync()
		{
			// Begin role update handler - only validate RoleForm fields here
			// Remove validation entries for all other fields so only RoleForm is validated
			var keysToKeepPrefix = "RoleForm.";
			var keysToRemove = ModelState.Keys.Where(k => !k.StartsWith(keysToKeepPrefix)).ToList();
			foreach (var k in keysToRemove) ModelState.Remove(k);
			if (!ModelState.IsValid)
			{
				await LoadPageAsync();
				return Page();
			}

			string targetRole = NormalizeRoleName(RoleForm.TargetRole);
			// If model binding failed to populate RoleForm, try reading values directly from Request.Form
			if (string.IsNullOrWhiteSpace(RoleForm.UserId))
			{
				if (Request?.Form != null)
				{
					var uid = Request.Form["RoleForm.UserId"].FirstOrDefault();
					var tr = Request.Form["RoleForm.TargetRole"].FirstOrDefault();
					if (!string.IsNullOrWhiteSpace(uid) || !string.IsNullOrWhiteSpace(tr))
					{
						RoleForm = new UserRoleInputModel
						{
							UserId = uid ?? string.Empty,
							TargetRole = tr ?? string.Empty
						};
						Console.WriteLine($"[DEBUG] Fallback read from Request.Form: UserId='{RoleForm.UserId}', TargetRole='{RoleForm.TargetRole}'");
					}
				}
				if (string.IsNullOrWhiteSpace(RoleForm.UserId))
				{
					ModelState.AddModelError(string.Empty, "Select a user before updating permissions.");
					await LoadPageAsync();
					return Page();
				}
			}

			if (targetRole is not GeneralRole and not WriterRole and not EditorRole and not AdminRole)
			{
				ModelState.AddModelError(string.Empty, "Choose a valid target role.");
				await LoadPageAsync();
				return Page();
			}

			ApplicationUser? user = await _userManager.FindByIdAsync(RoleForm.UserId);
			if (user is null)
			{
				ModelState.AddModelError(string.Empty, "The selected user could not be found.");
				await LoadPageAsync();
				return Page();
			}

			var currentUserId = _userManager.GetUserId(User);
			string? superAdminId = await GetPrimaryAdminUserIdAsync();
			bool isTargetAdmin = await _userManager.IsInRoleAsync(user, AdminRole);
			bool isTargetPrimaryAdmin = string.Equals(user.Id, superAdminId, StringComparison.Ordinal);
			if (isTargetAdmin && isTargetPrimaryAdmin && !string.Equals(currentUserId, superAdminId, StringComparison.Ordinal))
			{
				ModelState.AddModelError(string.Empty, "Admin accounts are read-only from this panel.");
				await LoadPageAsync();
				return Page();
			}

			await EnsureRoleAsync(AdminRole);
			await EnsureRoleAsync(WriterRole);
			await EnsureRoleAsync(EditorRole);
			await EnsureRoleAsync(GeneralRole);

			IEnumerable<string> currentRoles = await _userManager.GetRolesAsync(user);
			List<string> removableRoles = currentRoles.Where(role => role is AdminRole or WriterRole or EditorRole or GeneralRole).ToList();
			if (removableRoles.Count > 0)
			{
				_ = await _userManager.RemoveFromRolesAsync(user, removableRoles);
			}

			if (targetRole == AdminRole && isTargetPrimaryAdmin && !string.Equals(currentUserId, superAdminId, StringComparison.Ordinal))
			{
				ModelState.AddModelError(string.Empty, "Only the primary admin may grant admin privileges.");
				await LoadPageAsync();
				return Page();
			}

			if (targetRole != GeneralRole)
			{
				_ = await _userManager.AddToRoleAsync(user, targetRole);
			}

			await SyncLegacyReaderRoleAsync(user, targetRole);

			TempData["StatusMessage"] = $"Updated {user.UserName}'s permissions.";
			return RedirectToPage();
		}

		private async Task LoadPageAsync()
		{
			SiteSettings settings = await EnsureSiteSettingsAsync();
			SettingsForm = new AdminSettingsInputModel
			{
				StartYear = settings.StartYear,
				SchoolName = settings.SchoolName,
				SchoolAcronym = settings.SchoolAcronym,
				SchoolBlurb = settings.SchoolBlurb,
				SchoolLogo = settings.SchoolLogo,
				SchoolEmblem = settings.SchoolEmblem,
				PrimaryColor = settings.ColorScheme is null || string.IsNullOrWhiteSpace(settings.ColorScheme.Color1)
					? "#1B6EC2"
					: $"#{settings.ColorScheme.Color1.ToUpperInvariant()}",
				SecondaryColor = settings.ColorScheme is null || string.IsNullOrWhiteSpace(settings.ColorScheme.Color2)
					? "#F5A623"
					: $"#{settings.ColorScheme.Color2.ToUpperInvariant()}",
				JobPosition = settings.MediaContact?.JobPosition,
				FullName = settings.MediaContact?.FullName,
				Phone = settings.MediaContact?.Phone,
				Email = settings.MediaContact?.Email
			};

			OffSiteLinks = settings.OffSiteLinks
				.OrderBy(link => link.Name)
				.Select(link => new OffSiteLinkViewModel
				{
					Id = link.Id,
					Name = link.Name,
					Url = link.URL
				})
				.ToList();

			List<UserRowViewModel> users = await LoadUsersAsync();
			WriterUsers = users.Where(user => !user.IsAdmin && user.PrimaryRole == WriterRole).ToList();
			EditorUsers = users.Where(user => !user.IsAdmin && user.PrimaryRole == EditorRole).ToList();
			GeneralUsers = users.Where(user => !user.IsAdmin && user.PrimaryRole == GeneralRole).ToList();
			AdminUsers = users.Where(user => user.IsAdmin).ToList();

			PrimaryAdminUserId = AdminUsers.OrderBy(u => u.UserId, StringComparer.Ordinal).FirstOrDefault()?.UserId;
			var currentUserId = _userManager.GetUserId(User);
			IsCurrentUserSuperAdmin = string.Equals(currentUserId, PrimaryAdminUserId, StringComparison.Ordinal);
		}

		private async Task<string?> GetPrimaryAdminUserIdAsync()
		{
			IList<ApplicationUser> adminUsers = await _userManager.GetUsersInRoleAsync(AdminRole);
			List<string> adminUserIds = adminUsers.Select(user => user.Id).ToList();

			return adminUserIds.OrderBy(userId => userId, StringComparer.Ordinal).FirstOrDefault();
		}

		private async Task<List<UserRowViewModel>> LoadUsersAsync()
		{
			List<ApplicationUser> users = await _userManager.Users
				.OrderBy(user => user.UserName)
				.ToListAsync();

			List<UserRowViewModel> rows = [];
			foreach (ApplicationUser user in users)
			{
				IList<string> roles = await _userManager.GetRolesAsync(user);
				bool isAdmin = roles.Contains(AdminRole, StringComparer.OrdinalIgnoreCase);
				string primaryRole = roles.Contains(EditorRole, StringComparer.OrdinalIgnoreCase)
					? EditorRole
					: roles.Contains(WriterRole, StringComparer.OrdinalIgnoreCase)
						? WriterRole
						: GeneralRole;

				rows.Add(new UserRowViewModel
				{
					UserId = user.Id,
					UserName = user.UserName ?? user.Email ?? user.Id,
					Email = user.Email,
					Roles = roles.OrderBy(role => role).ToList(),
					PrimaryRole = primaryRole,
					IsAdmin = isAdmin
				});
			}

			return rows;
		}

		private async Task<SiteSettings> EnsureSiteSettingsAsync()
		{
			SiteSettings? settings = await _context.SiteSettings
				.Include(site => site.MediaContact)
				.Include(site => site.ColorScheme)
				.Include(site => site.OffSiteLinks)
				.FirstOrDefaultAsync();

			if (settings is null)
			{
				settings = new SiteSettings
				{
					StartYear = DateTime.UtcNow.Year,
					SchoolName = "School Name",
					SchoolAcronym = "ABCD",
					SchoolBlurb = "Welcome to our blog!",
					SchoolLogo = "/images/placeholder-logo.svg",
					SchoolEmblem = "/images/placeholder-emblem.svg",
					MediaContact = new MediaContact(),
					ColorScheme = new ColorScheme
					{
						Color1 = "1B6EC2",
						Color2 = "F5A623"
					}
				};

				_ = _context.SiteSettings.Add(settings);
				_ = await _context.SaveChangesAsync();
			}

			return settings;
		}

		private async Task EnsureRoleAsync(string roleName)
		{
			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				_ = await _roleManager.CreateAsync(new IdentityRole(roleName));
			}
		}

		private static string NormalizeHex(string? value)
		{
			string normalized = (value ?? string.Empty).Trim();
			if (normalized.StartsWith('#'))
			{
				normalized = normalized[1..];
			}

			return normalized.ToUpperInvariant();
		}

		private static string NormalizeRoleName(string? roleName)
		{
			return (roleName ?? string.Empty).Trim();
		}

		private async Task SyncLegacyReaderRoleAsync(ApplicationUser user, string targetRole)
		{
			string? readerUsername = user.UserName ?? user.Email;
			if (string.IsNullOrWhiteSpace(readerUsername))
			{
				return;
			}

			Reader? reader = await _context.Readers.FirstOrDefaultAsync(item => item.Username == readerUsername);
			if (reader is null)
			{
				reader = new Reader
				{
					Username = readerUsername,
					Password = null,
					IsWriter = false,
					IsEditor = false
				};

				_ = _context.Readers.Add(reader);
			}

			reader.IsWriter = targetRole == WriterRole;
			reader.IsEditor = targetRole == EditorRole;
			_ = await _context.SaveChangesAsync();
		}

		public sealed class AdminSettingsInputModel
		{
			[Range(1000, 9999)]
			public int StartYear { get; set; }

			[Required]
			[StringLength(80)]
			public string SchoolName { get; set; } = string.Empty;

			[Required]
			[StringLength(10)]
			[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "SchoolAcronym must contain letters only.")]
			public string SchoolAcronym { get; set; } = string.Empty;

			[Required]
			[StringLength(500)]
			public string SchoolBlurb { get; set; } = string.Empty;

			[Required]
			[StringLength(500)]
			public string SchoolLogo { get; set; } = string.Empty;

			[Required]
			[StringLength(500)]
			public string SchoolEmblem { get; set; } = string.Empty;

			[StringLength(80)]
			public string? JobPosition { get; set; }

			[StringLength(80)]
			public string? FullName { get; set; }

			[StringLength(20)]
			public string? Phone { get; set; }

			[StringLength(254)]
			[EmailAddress(ErrorMessage = "Email must be a valid email address.")]
			public string? Email { get; set; }

			[Required]
			[StringLength(7, MinimumLength = 7)]
			[RegularExpression(@"^#?[0-9A-Fa-f]{6}$", ErrorMessage = "Enter a valid hex color such as #1B6EC2.")]
			public string PrimaryColor { get; set; } = "#1B6EC2";

			[Required]
			[StringLength(7, MinimumLength = 7)]
			[RegularExpression(@"^#?[0-9A-Fa-f]{6}$", ErrorMessage = "Enter a valid hex color such as #F5A623.")]
			public string SecondaryColor { get; set; } = "#F5A623";
		}

		public sealed class OffSiteLinkInputModel
		{
			[Required]
			[StringLength(80)]
			public string Name { get; set; } = string.Empty;

			[Required]
			[StringLength(2083)]
			[Url(ErrorMessage = "URL must be a valid URL.")]
			public string Url { get; set; } = string.Empty;
		}

		public sealed class UserRoleInputModel
		{
			[Required]
			public string UserId { get; set; } = string.Empty;

			[Required]
			public string TargetRole { get; set; } = string.Empty;
		}

		public sealed class UserRowViewModel
		{
			public required string UserId { get; init; }

			public required string UserName { get; init; }

			public string? Email { get; init; }

			public IReadOnlyList<string> Roles { get; init; } = [];

			public required string PrimaryRole { get; init; }

			public bool IsAdmin { get; init; }
		}

		public sealed class OffSiteLinkViewModel
		{
			public int Id { get; init; }

			public required string Name { get; init; }

			public required string Url { get; init; }
		}
	}
}