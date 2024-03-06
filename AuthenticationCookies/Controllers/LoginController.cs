using ApplicationContext;
using AuthenticationCookies.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationCookies.Controllers
{
    public class LoginController : Controller
	{

		private readonly AppDbContext _appDbContext;

		public LoginController(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public IActionResult Index(Login login)
		{
			return View(new Login());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(Login login)
		{
			if (ModelState.IsValid)
			{
				var user = _appDbContext.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

				if (user != null)
				{
					HttpContext.Response.Cookies.Append("user", user.Password);

					Authenticate(login.Email);

					return RedirectToAction("Welcome","Admin");
				}

				ModelState.AddModelError("", "Некорректные логин и(или) пароль");
			}
			return View("Index", login);
		}


		public void Authenticate(string email)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, email)
			};
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

			HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

		}

		public IActionResult LoginOut()
		{
			HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

	}
}

