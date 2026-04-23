using System.ComponentModel.DataAnnotations;

namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents a reader record used for development and seed data.
	/// </summary>
	public class Readers
	{
		/// <summary>
		/// Primary key for the reader entry.
		/// </summary>
		[Key]
		public int UserID { get; set; }

		/// <summary>
		/// Username for the reader (seed data only).
		/// </summary>
		public required string Username { get; set; }

		/// <summary>
		/// Password placeholder for development seed data. Not used for authentication in production.
		/// </summary>
		public required string Password { get; set; }
	}
}
