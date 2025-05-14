using FlagItUpApp.Models;
using FlagItUpApp.Repositories;
using FlagItUpApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using FlagItUpApp.Extensions;
using Microsoft.EntityFrameworkCore;
using FlagItUpApp.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Flag>, FlagRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<FlagService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddRazorPages();
builder.Services.AddSession(); // 


builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<DbStorage>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseAuthentication(); 
app.UseAuthorization();

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapDefaultControllerRoute(); 

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Game}/{action=Start}/{id?}");
app.UseSession();
app.MapControllers();


app.Run();
