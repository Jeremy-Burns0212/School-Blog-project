using System.ComponentModel.DataAnnotations;

namespace School_Blog_project.Models
{
	public class Categories
	{
		/// <summary>
		/// the unique identifier for the category. This is the primary key for the Categories table.
		/// </summary>
		[Key]
		public int CatagoryId { get; set; }

		/// <summary>
		/// Gets or sets the abbreviated title for the item.
		/// </summary>
		/// <remarks>The short title is limited to a maximum of 10 characters. This property is required and must be
		/// set to a non-null, non-empty value.</remarks>
		[StringLength(10)]
		public required string ShortTitle { get; set; }

		/// <summary>
		/// Gets or sets the full title i.e. the complete name of the category. This property is required and must be set to a non-null, non-empty value.
		/// </summary>
		public required string FullTitle { get; set; }

		// Navigation collection for many-to-many linking to articles
		public ICollection<ArticleCategory> ArticleCatagories { get; set; } = [];
	}
}
