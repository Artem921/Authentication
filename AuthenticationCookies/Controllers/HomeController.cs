using Microsoft.AspNetCore.Mvc;

namespace AuthenticationCookies.Controllers;

public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

	}