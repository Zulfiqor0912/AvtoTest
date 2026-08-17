using AvtoTest.Service.Services;
using AvtoTest.Service.Services.Interfece;
using Microsoft.AspNetCore.Identity;
using AvtoTest.Data.Context;
using AvtoTest.Data.Entities;
using AvtoTest.Data.Repositories;
using AvtoTest.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using AvtoTest.Service.Services.Interfeces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<TestService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<IHomeRepasitory, HomeRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();


builder.Services.AddDbContext<AppDbContext>( options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentity<CustomUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();

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

//app.MapGet("/", context =>
//{
//    context.Response.Redirect("/Identity/Account/Login");
//    return Task.CompletedTask;
//});

app.MapRazorPages();

using (var scopeService = app.Services.CreateScope())
{
    var role = "Admin";
    var roleManager = scopeService.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roleModel = await roleManager.FindByNameAsync(role);

    if (roleModel is null)
    {
        roleModel = new()
        {
            Name = role
        };
        await roleManager.CreateAsync(roleModel);
    }

    var userManager = scopeService.ServiceProvider.GetRequiredService<UserManager<CustomUser>>();

    var email = "admin@admin.com";
    var password = "Jav#12";

    var user = await userManager.FindByEmailAsync(email);

    if (user is null)
    {
        user = new()
        {
            Email = email,
            UserName = email
        };

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, role);
    }
}

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=EntryPage}/{id?}")
.WithStaticAssets();


app.Run();
