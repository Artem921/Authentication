using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Database.Controllers
{
    public class WelcomeController : Controller
    {
        [Authorize]
        public IActionResult AdminWelcome()
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
