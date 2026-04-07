namespace School_Blog_project.Models
{
	public class Readers
	{
		public int UserID { get; set; }

		public required string Username { get; set; }

		public required string Password { get; set; }

		[Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
		public bool IsWriter { get; set; }

		[Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
		public bool IsEditor { get; set; }
	}
}
