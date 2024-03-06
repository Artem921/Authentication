
using Authorization.VKontakte.Models;
using IdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Authorization.VKontakte.Controllers
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

        public async Task<IActionResult> IndexAsync()
        {
            var externalProvider= await _signInManager.GetExternalAuthenticationSchemesAsync();
            return View(new Login
            {
                ExternalProvider=externalProvider
            });
        }

		public async Task<IActionResult> ExternalLogin(string provider,string returnUrl)
		{
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback),"Administrator",new {returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties,provider);
			
		}

        public async Task< IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                return RedirectToAction("Index");
            }

            var result=await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
            if (result.Succeeded)
            {

            }
            return RedirectToAction("RegisterExternal", new ExternalLogin
            {
                ReturnUrl = returnUrl,
                Email=info.Principal.FindFirstValue(ClaimTypes.Name)
            });
        }

        public IActionResult RegisterExternal(ExternalLogin login)
        {
            return View(login);
        }

        [HttpPost]
        [ActionName("RegisterExternal")]
		public async Task<IActionResult> RegisterExternalConfirmedAsync(ExternalLogin login)
		{
            var info=await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Index");
            }

            var user = new AppUser(login.Email);
            var result =await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var claims=await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, Constant.RoleNames.Admin));
                if(claims.Succeeded)
                {
                   var identity= await _userManager.AddLoginAsync(user, info);
                    if(identity.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
						return RedirectToAction("Index", "Home");

					}
                }
            }
			return View(login);
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
            return View("Index");

        }




        public IActionResult LoginOut()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}

