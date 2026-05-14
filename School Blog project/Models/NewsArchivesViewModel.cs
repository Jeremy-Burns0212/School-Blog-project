namespace School_Blog_project.Models
{
	/// <summary>
	/// Represents the data required to display and filter news articles by year in an archive view.
	/// </summary>
	public sealed class NewsArchivesViewModel
	{
		/// <summary>
		/// The years for which news articles are available. This list is generated based on the 
		/// publication years of articles in the database.
		/// </summary>
		public IReadOnlyList<int> Years { get; init; } = [];

		/// <summary>
		/// The currently selected year for filtering news articles. This value is used to indicate 
		/// which year's articles are being displayed. If null, it indicates that no specific year 
		/// filter is applied and all news articles are shown.
		/// </summary>
		public int? SelectedYear { get; init; }
	}
}