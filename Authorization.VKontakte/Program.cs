using Microsoft.EntityFrameworkCore;
using IdentityDb;
using IdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
using Authorization.VKontakte;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppIdentityContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Authentication")))
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppIdentityContext>();

builder.Services.AddAuthentication()
  
    .AddOAuth("VK", "VKontakte", config =>
    {
        config.ClientId = ConfigurationApplication.AppSetting["Authentication:VKontakte:AppId"];
        config.ClientSecret = ConfigurationApplication.AppSetting["Authentication:VKontakte:AppSecret"];
        config.ClaimsIssuer = "VKontakte";
        config.CallbackPath = new PathString("/signin-vkontakte-token");
        config.AuthorizationEndpoint = "https//oauth.vk.com/authorize";
        config.TokenEndpoint = "https//oauth.vk.com/access_token";
        config.Scope.Add("email");
        config.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        config.SaveTokens = true;
        config.Events = new OAuthEvents
        {
            OnCreatingTicket = context =>
            {
                context.RunClaimActions(context.TokenResponse.Response.RootElement);
                return Task.CompletedTask;
            },
            OnRemoteFailure = OnFailer
        };
        

	});
 Task OnFailer(RemoteFailureContext context)
{

	return Task.CompletedTask;
}

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Home/Index";
});



var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    IdentityInitializer.Initialize(userManager, roleManager);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
