using Microsoft.AspNetCore.Mvc;
using School_Blog_project.Models;
using System.Diagnostics;

namespace School_Blog_project.Controllers
{
	/// <summary>
	/// Controller for basic site pages such as Index, Privacy and informational pages.
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// Returns the home page view.
		/// </summary>
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Returns the privacy page view.
		/// </summary>
		public IActionResult Privacy()
		{
			return View();
		}

		/// <summary>
		/// Returns the students information page view.
		/// </summary>
		public IActionResult Students()
		{
			return View();
		}

		/// <summary>
		/// Returns the graduates information page view.
		/// </summary>
		public IActionResult Graduates()
		{
			return View();
		}

		/// <summary>
		/// Returns the faculty information page view.
		/// </summary>
		public IActionResult Faculty()
		{
			return View();
		}

		/// <summary>
		/// Returns the news listing view.
		/// </summary>
		public IActionResult News()
		{
			return View();
		}

		/// <summary>
		/// Displays an error page with request information.
		/// </summary>
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
