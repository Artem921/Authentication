using ApplicationContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ApplicationContext.Entities;
using Authentication.Roles.Constant;
using Authentication.Roles.Models;

namespace Authentication.Roles.Controllers
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
        public ActionResult Login()
        {
            return View();
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

                    Authenticate(user);

                    if (user.Role == RoleNames.Admin) return RedirectToAction("AdminWelcome", "Welcome");

                    if (user.Role == RoleNames.User) return RedirectToAction("UserWelcome", "Welcome");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View("Index", login);
        }


        public void Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Email ),
                new Claim(ClaimTypes.Role,user.Role)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationRoles", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }

        public IActionResult LoginOut()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}

