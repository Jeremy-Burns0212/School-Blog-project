using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace School_Blog_project.Pages.WriterOnly
{
	[Authorize(Policy = "IsWriter")]
	public class IndexModel : PageModel
	{
		public void OnGet()
		{
		}
	}
}
