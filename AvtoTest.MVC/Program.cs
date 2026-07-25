using AvtoTest.Data.Repositories;
using AvtoTest.Data.Repositories.Interfaces;
using AvtoTest.Service.Services;
using AvtoTest.Service.Services.Interfece;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AvtoTest.Data.Context;
using AvtoTest.Data.Entities;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AddDbContext") ?? throw new InvalidOperationException("Connection string 'AddDbContext' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<TestService>();

builder.Services.AddDbContext<AddDbContext>( options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<CustomUser, IdentityRole>().AddEntityFrameworkStores<AddDbContext>()
    .AddDefaultTokenProviders();


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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Test}/{action=Tickets}/{id?}")
    .WithStaticAssets();


app.Run();
