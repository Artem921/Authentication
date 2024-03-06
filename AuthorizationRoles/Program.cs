using ApplicationContext;
using Authentication.Roles.Constant;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("Authentication")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Home/Index";
		options.AccessDeniedPath = "/Home/AccessDenied";
		options.ExpireTimeSpan = TimeSpan.FromDays(1);
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(RoleNames.Admin, builder =>
	{
		builder.RequireClaim(ClaimTypes.Role, RoleNames.Admin);
	});

	options.AddPolicy(RoleNames.User, builder =>
	{
		builder.RequireClaim(ClaimTypes.Role, RoleNames.User);
	});

});

var app = builder.Build();

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
