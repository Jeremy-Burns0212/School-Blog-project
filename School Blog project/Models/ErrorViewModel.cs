namespace School_Blog_project.Models
{
	/// <summary>
	/// View model used to render error information to the user.
	/// </summary>
	public class ErrorViewModel
	{
		/// <summary>
		/// The request identifier associated with the error (if available).
		/// </summary>
		public string? RequestId { get; set; }

		/// <summary>
		/// Determines whether a request id should be shown on the error page.
		/// </summary>
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
