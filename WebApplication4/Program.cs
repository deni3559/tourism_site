using Microsoft.EntityFrameworkCore;
using System;
using WebPortal.Controllers;
using WebPortal.CustomMiddleware;
using WebPortal.DbStuff;
using WebPortal.DbStuff.Repositories;
using WebPortal.DbStuff.Repositories.Interfaces;
using WebPortal.Hubs;
using WebPortal.Services;
using WebPortal.Services.Apis;
using WebPortal.Services.Permissions;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpLogging(opt => opt.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All);
builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);

builder.Services
    .AddAuthentication(AuthController.AUTH_KEY)
    .AddCookie(AuthController.AUTH_KEY, o =>
    {
        o.LoginPath = "/Auth/Login";
        o.ForwardForbid = "/Auth/Forbid ";
    });

// Register db context
string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = TourismDB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate=False; Application Intent = ReadWrite; Multi Subnet Failover=False";
builder.Services.AddDbContext<TourismPortalContext>(x => x.UseSqlServer(connectionString));

// Register Repositories
builder.Services.AddScoped<IUserRepositrory, UserRepositrory>();
builder.Services.AddScoped<ITourPermission, TourPermission>();
builder.Services.AddScoped<IArticlePermission, ArticlePermission>();
builder.Services.AddScoped<ITourismFilesService, TourismFilesService>();
builder.Services.AddScoped<INextArticlePreviewRepository, NextArticlePreviewRepository>();
builder.Services.AddScoped<IShopCartRepository, ShopCartRepository>();
builder.Services.AddScoped<IToursRepository, ToursRepository>();

builder.Services.AddHttpClient<WikiPageApi>(client =>
{
    client.BaseAddress = new Uri("https://en.wikipedia.org/");
    client.DefaultRequestHeaders.Add("User-Agent", "WebPortalApp/1.0");
});

builder.Services.AddScoped<SeedService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seed = scope.ServiceProvider.GetRequiredService<SeedService>();
    seed.Seed();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()
    && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower() != "development")
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Tourism/NotFound");
app.UseRouting();

// Who am I?
app.UseAuthentication();
// What can I do?
app.UseAuthorization();

var localizationMode = builder.Configuration["Localization:Mode"];

app.UseMiddleware<CustomLocalizationMiddleware>();

app.MapHub<TourNotificationHub>("/hubs/notification/tourism");

// Enable attribute-routed API controllers like /api/CatalogApi
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tourism}/{action=Index}/{id?}");

app.Run();
