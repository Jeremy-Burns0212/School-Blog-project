using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents the site settings and configuration for the School Blog.
	/// </summary>
	public class SiteSettings
	{
		/// <summary>
		/// Primary key for the site settings.
		/// </summary>
		public int SiteSettingsId { get; set; }

		/// <summary>
		/// The year the school was founded. Must be a 4-digit year.
		/// </summary>
		[Required]
		[Range(1000, 9999, ErrorMessage = "StartYear must be a valid 4-digit year.")]
		public int StartYear { get; set; } = 2010;

		/// <summary>
		/// The name of the school.
		/// </summary>
		[Required]
		[StringLength(80)]
		public string SchoolName { get; set; } = "School Name";

		/// <summary>
		/// The acronym for the school. Must contain letters only.
		/// </summary>
		[Required]
		[StringLength(10)]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "SchoolAcronym must contain letters only.")]
		public string SchoolAcronym { get; set; } = "ABCD";

		/// <summary>
		/// A brief description or welcome message for the school.
		/// </summary>
		[Required]
		[StringLength(500)]
		public string SchoolBlurb { get; set; } = "Welcome to our blog!";

		/// <summary>
		/// The file path to the school logo image.
		/// </summary>
		[Required]
		[StringLength(500)]
		public string SchoolLogo { get; set; } = "/images/placeholder-logo.svg";

		/// <summary>
		/// The file path to the school emblem image.
		/// </summary>
		[Required]
		[StringLength(500)]
		public string SchoolEmblem { get; set; } = "/images/placeholder-emblem.svg";

		/// <summary>
		/// Navigation property for the media contact information.
		/// </summary>
		public MediaContact? MediaContact { get; set; }

		/// <summary>
		/// Navigation property for the color scheme.
		/// </summary>
		public ColorScheme? ColorScheme { get; set; }

		/// <summary>
		/// Navigation property for off-site links.
		/// </summary>
		public ICollection<OffSiteLink> OffSiteLinks { get; set; } = [];
	}

	/// <summary>
	/// Represents media contact information for the school.
	/// Has a one-to-one relationship with SiteSettings.
	/// </summary>
	public class MediaContact
	{
		/// <summary>
		/// Foreign key and primary key for the media contact. References SiteSettings.
		/// </summary>
		[Key]
		[ForeignKey(nameof(SiteSettings))]
		public int SiteSettingsId { get; set; }

		/// <summary>
		/// The job position of the media contact.
		/// </summary>
		[StringLength(80)]
		public string? JobPosition { get; set; }

		/// <summary>
		/// The full name of the media contact.
		/// </summary>
		[StringLength(80)]
		public string? FullName { get; set; }

		/// <summary>
		/// The phone number of the media contact. Must be a valid phone number format.
		/// </summary>
		[StringLength(20)]
		[Phone(ErrorMessage = "Phone must be a valid phone number.")]
		public string? Phone { get; set; }

		/// <summary>
		/// The email address of the media contact. Must be a valid email format.
		/// </summary>
		[StringLength(254)]
		[EmailAddress(ErrorMessage = "Email must be a valid email address.")]
		public string? Email { get; set; }

		/// <summary>
		/// Navigation property for the associated site settings.
		/// </summary>
		public SiteSettings? SiteSettings { get; set; }
	}

	/// <summary>
	/// Represents the color scheme for the school website.
	/// Has a one-to-one relationship with SiteSettings.
	/// </summary>
	public class ColorScheme
	{
		/// <summary>
		/// Foreign key and primary key for the color scheme. References SiteSettings.
		/// </summary>
		[Key]
		[ForeignKey(nameof(SiteSettings))]
		public int SiteSettingsId { get; set; }

		/// <summary>
		/// The primary color as a 6-character hexadecimal value (without #).
		/// </summary>
		[Required]
		[StringLength(6, MinimumLength = 6)]
		[RegularExpression(@"^[0-9A-Fa-f]{6}$", ErrorMessage = "Color1 must be a valid 6-character hexadecimal value.")]
		public string Color1 { get; set; } = string.Empty;

		/// <summary>
		/// The secondary color as a 6-character hexadecimal value (without #).
		/// </summary>
		[Required]
		[StringLength(6, MinimumLength = 6)]
		[RegularExpression(@"^[0-9A-Fa-f]{6}$", ErrorMessage = "Color2 must be a valid 6-character hexadecimal value.")]
		public string Color2 { get; set; } = string.Empty;

		/// <summary>
		/// Navigation property for the associated site settings.
		/// </summary>
		public SiteSettings? SiteSettings { get; set; }
	}

	/// <summary>
	/// Represents an off-site link to be displayed on the website.
	/// Has a one-to-many relationship with SiteSettings.
	/// </summary>
	public class OffSiteLink
	{
		/// <summary>
		/// Primary key for the off-site link.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Foreign key referencing the associated SiteSettings.
		/// </summary>
		[Required]
		public int SiteSettingsId { get; set; }

		/// <summary>
		/// The display name of the off-site link.
		/// </summary>
		[Required]
		[StringLength(80)]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// The URL of the off-site link. Must be a valid URL.
		/// </summary>
		[Required]
		[StringLength(2083)]
		[Url(ErrorMessage = "URL must be a valid URL.")]
		public string URL { get; set; } = string.Empty;

		/// <summary>
		/// Navigation property for the associated site settings.
		/// </summary>
		public SiteSettings? SiteSettings { get; set; }
	}
}
