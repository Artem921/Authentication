
using Authentication.Roles.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Roles.Controllers
{
    public class WelcomeController : Controller
    {
        [Authorize(Roles = RoleNames.Admin)]
        public IActionResult AdminWelcome()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Email = HttpContext.User.Identity.Name;
                ViewBag.Password = HttpContext.Request.Cookies["user"];
            }
            return View();
        }
        [Authorize(Roles = RoleNames.User)]
        public IActionResult UserWelcome()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Email = HttpContext.User.Identity.Name;
                ViewBag.Password = HttpContext.Request.Cookies["user"];
            }
            return View();
        }


    }
}
