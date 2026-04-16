namespace School_Blog_project.Models
{
	public class Reader
	{
		public int UserID { get; set; }

		public required string Username { get; set; }

		// NOTE: Passwords in seed data are placeholders for development only
		public string? Password { get; set; }

		public bool IsWriter { get; set; }

		public bool IsEditor { get; set; }
	}
}
