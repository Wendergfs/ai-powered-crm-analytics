
using Microsoft.EntityFrameworkCore;
using AIClientManager.Data;
using AIClientManager.Services;
using QuestPDF.Infrastructure;
using AIClientManager.Models;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DashboardChartService>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IClientAnalysisService,
    RuleBasedClientAnalysisService>();
builder.Services.AddHttpClient<IClientAnalysisService, OllamaClientAnalysisService>();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});


var app = builder.Build();
app.UseDeveloperExceptionPage();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Clients}/{action=Index}/{id?}");
app.Run();
