using Authentication.Jwt.Models;
using IdentityDb.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Jwt.Controllers
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
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                Console.WriteLine(HttpContext.User.Identity.IsAuthenticated);
                Console.WriteLine(HttpContext.User.Identity.AuthenticationType);
                if (user != null)
                {
                    GenerateToken(user);
                    return RedirectToAction("Index", "Home");
                }


                else
                {
                    ModelState.AddModelError("Email", "Такого пользователя нет");
                    return View("Index", login);
                }

            }
            return View("Index", login);

        }


        public void GenerateToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),

            };
            byte[] secretBytes = Encoding.UTF8.GetBytes(ConfigurationApplication.AppSetting["JWT:Secret"]);
            var key = new SymmetricSecurityKey(secretBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: ConfigurationApplication.AppSetting["JWT:ValidIssuer"],
                audience: ConfigurationApplication.AppSetting["JWT:ValidAudience"],
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            HttpContext.Response.Cookies.Append("Token", tokenString);


        }

        public IActionResult LoginOut()
        {
            HttpContext.Response.Cookies.Delete("Token");

            return RedirectToAction("Index", "Home");
        }
    }
}
