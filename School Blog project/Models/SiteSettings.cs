using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Blog_project.Models
{
	public class SiteSettings
	{
		[Key]
		public int SiteSettingsId { get; set; }

		[Required]
		[Range(2010, 9999, ErrorMessage = "Year must be a 4-digit number >= 2010.")]
		public int StartYear { get; set; } = 2010;

		[Required]
		[MaxLength(80)]
		public string SchoolName { get; set; } = "School Name";

		[Required]
		[MaxLength(10)]
		[RegularExpression("^[A-Za-z]+$", ErrorMessage = "Acronym must contain letters only.")]
		public string SchoolAcronym { get; set; } = "ABCD";

		[Required]
		[MaxLength(500)]
		public string SchoolBlurb { get; set; } = "Welcome to our blog!";

		[Required]
		public string SchoolLogo { get; set; } = "/images/placeholder-logo.png";

		[Required]
		public string SchoolEmblem { get; set; } = "/images/placeholder-emblem.png";

		public MediaContact MediaContact { get; set; } = null!;
		public ColorScheme ColorScheme { get; set; } = null!;
		public ICollection<OffSiteLink> OffSiteLinks { get; set; } = [];
	}

	public class MediaContact
	{
		[Key, ForeignKey("SiteSettings")]
		public int SiteSettingsId { get; set; }

		[MaxLength(80)]
		public string? JobPosition { get; set; }

		[MaxLength(80)]
		public string? FullName { get; set; }

		[Phone]
		[MaxLength(20)]
		public string? Phone { get; set; }

		[EmailAddress]
		[MaxLength(254)]
		public string? Email { get; set; }

		public SiteSettings SiteSettings { get; set; } = null!;
	}

	public class ColorScheme
	{
		[Key, ForeignKey("SiteSettings")]
		public int SiteSettingsId { get; set; }

		[Required]
		[StringLength(6, MinimumLength = 6)]
		[RegularExpression("^[0-9A-Fa-f]{6}$", ErrorMessage = "Must be a 6-digit hex value.")]
		public string Color1 { get; set; } = null!;

		[Required]
		[StringLength(6, MinimumLength = 6)]
		[RegularExpression("^[0-9A-Fa-f]{6}$", ErrorMessage = "Must be a 6-digit hex value.")]
		public string Color2 { get; set; } = null!;

		public SiteSettings SiteSettings { get; set; } = null!;
	}

	public class OffSiteLink
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("SiteSettings")]
		public int SiteSettingsId { get; set; }

		[Required]
		[MaxLength(80)]
		public string Name { get; set; } = null!;

		[Required]
		[MaxLength(2083)]
		[Url]
		public string URL { get; set; } = null!;

		public SiteSettings SiteSettings { get; set; } = null!;
	}
}
