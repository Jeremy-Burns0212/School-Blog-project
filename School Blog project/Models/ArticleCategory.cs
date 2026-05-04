namespace School_Blog_project.Models
{
	public class ArticleCategory
	{
		/// <summary>
		/// The unique identifier for the article. This is a foreign key referencing the ArticleID in the Articles table.
		/// </summary>
		public int ArticleID { get; set; }

		/// <summary>
		/// The unique identifier for the category. This is a foreign key referencing the CatagoryId in the Categories table.
		/// </summary>
		public int CatagoryId { get; set; }

		/// <summary>
		/// The article is associated with this category. This is a navigation property that allows access to the related Article entity.
		/// </summary>
		public Article? Article { get; set; }

		/// <summary>
		/// The category associated with this article. This is a navigation property that allows access to the related Categories entity.
		/// </summary>
		/// </summary>
		public Categories? Category { get; set; }
	}
}
