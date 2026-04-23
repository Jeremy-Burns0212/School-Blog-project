using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents a persistent reader record used by the application seed data.
	/// </summary>
	public class Reader
	{
		/// <summary>
		/// Primary key for the reader.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserID { get; set; }

		/// <summary>
		/// Username for the reader.
		/// </summary>
		public required string Username { get; set; }

		/// <summary>
		/// Optional password placeholder used only for development seed data.
		/// </summary>
		// NOTE: Passwords in seed data are placeholders for development only
		public string? Password { get; set; }

		/// <summary>
		/// Indicates whether the reader has writer privileges.
		/// </summary>
		public bool IsWriter { get; set; }

		/// <summary>
		/// Indicates whether the reader has editor privileges.
		/// </summary>
		public bool IsEditor { get; set; }
	}
}
