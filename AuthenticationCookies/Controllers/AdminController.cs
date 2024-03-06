using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationCookies.Controllers;

public class AdminController : Controller
	{
		[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
		public IActionResult Welcome()
		{
			if (User.Identity.IsAuthenticated)
			{
				ViewBag.Email = HttpContext.User.Identity.Name;
				
				ViewBag.Password = HttpContext.Request.Cookies["user"];

        }
			var principal = HttpContext.User ;

			return View(principal);
		}
	}
