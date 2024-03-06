using Authentication.Jwt;
using IdentityDb;
using IdentityDb.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppIdentityContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Authentication")))
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppIdentityContext>(); ;

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( config =>
    {
        byte[] secretBytes = Encoding.UTF8.GetBytes(ConfigurationApplication.AppSetting["JWT:Secret"]);
        var key = new SymmetricSecurityKey(secretBytes);

        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = ConfigurationApplication.AppSetting["JWT:ValidIssuer"],
            ValidAudience = ConfigurationApplication.AppSetting["JWT:ValidAudience"],
            IssuerSigningKey = key
        };

        config.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["Token"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.MaxAge = TimeSpan.FromDays(2);
    options.LoginPath = "/Login/Login";
    options.SlidingExpiration = true;

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    IdentityInitializer.Initialize(userManager, roleManager);
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllerRoute(
        name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
