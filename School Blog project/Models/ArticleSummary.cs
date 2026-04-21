namespace School_Blog_project.Models
{
	public class ArticleSummary
	{
		public required string Title { get; set; }
		public required DateTime Date { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }

		// Support multiple categories
		public List<string> Categories { get; set; } = new();
	}
}
