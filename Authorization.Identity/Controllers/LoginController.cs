using Authentication.Database.Models;
using IdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Database.Controllers
{
    public class LoginController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(Login login)
        {
            return View(new Login());
        }


        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Такого пользователя нет");
                    return View("Index", login);
                }
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
            }
            return View("Index", login);

        }




        public IActionResult LoginOut()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}

