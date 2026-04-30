using Microsoft.EntityFrameworkCore;
using hrbs_project.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services BEFORE build
builder.Services.AddControllersWithViews();

//builder.Services.AddSession();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();
var app = builder.Build();

builder.Services.AddSession();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
//app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Enable session (IMPORTANT)
app.UseSession();

app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();