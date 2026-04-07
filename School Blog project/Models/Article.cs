namespace School_Blog_project.Models
{
	public class Article
	{
		public int ArticleID { get; set; }

		public required string Title { get; set; }

		public string? Author { get; set; }

		public DateTime DatePublished { get; set; }

		public string? ArticleImagePath { get; set; }
	}
}