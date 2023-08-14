using EduHome.Contexts;
using EduHome.Models.Identity;
using EduHome.Services.Implementations;
using EduHome.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AppDbContextInitializer>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireDigit = true;
	options.Password.RequireNonAlphanumeric = true;

	options.User.RequireUniqueEmail = true;

	options.SignIn.RequireConfirmedEmail = true;

	options.Lockout.MaxFailedAccessAttempts = 3;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
	options.Lockout.AllowedForNewUsers = false;
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

//builder.Services.Configure<IdentityOptions>(
//	options => options.SignIn.RequireConfirmedEmail = true
//);
 
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(30);
//});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Auth/LogIn";
});

//Constants.mail = builder.Configuration["MailSettings:Mail"];
//Constants.password = builder.Configuration["MailSettings:Password"];
//Constants.host = builder.Configuration["MailSettings:Host"];
//Constants.port = int.Parse(builder.Configuration["MailSettings:Port"]);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

using(var scope = app.Services.CreateScope())
{
	var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
	await initializer.InitializeAsync();
	await initializer.UserSeedAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();