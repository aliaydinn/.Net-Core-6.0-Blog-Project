using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetCoreBlog.Data.Context;
using NetCoreBlog.Data.Extensions;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Service.CustomValidations;
using NetCoreBlog.Service.Desribers;
using NetCoreBlog.Service.Extensions;
using NetCoreBlog.Web.Filters.ArticleVisitors;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.LoadDataLayerExtension(builder.Configuration);
builder.Services.LoadServiceLayerExtension();
builder.Services.AddSession();
builder.Services.AddControllersWithViews(opt=>{
    opt.Filters.Add<ArticleVisitorFilter>();
})
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 5000
    })
    .AddRazorRuntimeCompilation();


//Canlýya taþýndýðýnda kaldýrýlacak.
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequiredLength = 6;
    opt.Password.RequiredUniqueChars = 1;
})
    .AddPasswordValidator<CustomPasswordValidation>()
.AddRoleManager<RoleManager<AppRole>>()
.AddErrorDescriber<CustomIdentityErrorDesriber>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/Login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "NetCoreBlog",
        HttpOnly = true,
        SecurePolicy = CookieSecurePolicy.SameAsRequest,
        SameSite = SameSiteMode.Strict
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan=TimeSpan.FromDays(7);
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccesDenied");

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{Id?}"
        );
    endpoints.MapDefaultControllerRoute();
});

app.Run();
