using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Jwt.Controllers
{
    public class WelcomeController : Controller
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AdminWelcome()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Token = HttpContext.Request.Cookies["Token"];


            }
            return View();
        }
   
    }
}

