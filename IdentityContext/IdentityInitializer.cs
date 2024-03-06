
using IdentityDb.Constant;
using IdentityDb.Entities;
using Microsoft.AspNetCore.Identity;


namespace IdentityDb
{
    public class IdentityInitializer
    {

    public static void Initialize(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
            var userEmail = "vova@mail.ru";
            var adminEmail = "admin@mail.ru";
            var password = "Aa123456!";

        if (roleManager.FindByNameAsync(RoleNames.Admin).Result == null)
        {
            roleManager.CreateAsync(new AppRole(RoleNames.Admin)).Wait();
        }

        if (roleManager.FindByNameAsync(RoleNames.User).Result == null)
        {
            roleManager.CreateAsync(new AppRole(RoleNames.User)).Wait();
        }

      

        if (userManager.FindByNameAsync(adminEmail).Result == null)
        {
            var admin = new AppUser { Email = adminEmail, UserName = adminEmail };
            var result = userManager.CreateAsync(admin, password).Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(admin, RoleNames.Admin).Wait();
            }
        }
            if (userManager.FindByNameAsync(userEmail).Result == null)
            {
                var user = new AppUser { Email = userEmail, UserName = userEmail };
                var result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, RoleNames.User).Wait();
                }
            }
        }
}
}
