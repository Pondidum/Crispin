using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Home
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
