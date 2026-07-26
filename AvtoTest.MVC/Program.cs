using AvtoTest.Service.Services;
using AvtoTest.Service.Services.Interfece;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AvtoTest.Data.Context;
using AvtoTest.Data.Entities;
using AvtoTest.Data.Repositories;
using AvtoTest.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<TestService>();

builder.Services.AddDbContext<AddDbContext>( options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<CustomUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<AddDbContext>()
    .AddDefaultTokenProviders();
    


builder.Services.AddRazorPages();

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

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Test}/{action=Tickets}/{id?}")
    .WithStaticAssets();


app.Run();
