using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace SchoolBlogProject.Pages.WriterOnly
{
	[Authorize(Policy = "IsWriter")]
	public class IndexModel : PageModel
	{
		public void OnGet()
		{
		}
	}
}
