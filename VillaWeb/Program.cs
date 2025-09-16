using Microsoft.AspNetCore.Authentication.Cookies;
using VillaWeb;
using VillaWeb.Models;
using VillaWeb.Service;
using VillaWeb.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<List<RoleItem>>(builder.Configuration.GetSection("AllowedRolesForRegistration"));
builder.Services.AddHttpClient<IUnitOfServices, UnitOfServices>();
builder.Services.AddScoped<IUnitOfServices, UnitOfServices>();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath  = "/Account/AccountHome/Login";
        options.LogoutPath = "/Account/AccountHome/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=CustomerHome}/{action=Index}/{id?}");

// app.MapControllerRoute(
//     name: "default_no_area",
//     pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
