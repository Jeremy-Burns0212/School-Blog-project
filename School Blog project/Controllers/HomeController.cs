using Microsoft.AspNetCore.Mvc;
using School_Blog_project.Models;
using System.Diagnostics;

namespace School_Blog_project.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Students()
		{
			return View();
		}

		public IActionResult Graduates()
		{
			return View();
		}

		public IActionResult Faculty()
		{
			return View();
		}

		public IActionResult News()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
