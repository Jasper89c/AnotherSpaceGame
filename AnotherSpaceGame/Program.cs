using AnotherSpaceGame.Data;
using AnotherSpaceGame.Hubs;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<TurnService>();
builder.Services.AddScoped<IUserStatusService, UserStatusService>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<PeriodicTableUpdater>();
builder.Services.AddHostedService<FederationUpdater>();
builder.Services.AddHostedService<TempleUpdater>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.Use(async (context, next) =>
{
    // 1. Get the user ID from claims
    var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (!string.IsNullOrEmpty(userId))
    {
        // 2. Create a scope to resolve scoped services
        using var scope = context.RequestServices.CreateScope();

        // 3. Get UserManager<ApplicationUser>
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 4. Retrieve the user from the database
        var user = await userManager.FindByIdAsync(userId);

        app.Use(async (context, next) =>
        {
            // 1. Get the user ID from claims
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // 2. Create a scope to resolve scoped services
                using var scope = context.RequestServices.CreateScope();

                // 3. Get UserManager<ApplicationUser>
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // 4. Retrieve the user from the database
                var user = await userManager.FindByIdAsync(userId);

                // Now you can access additional properties, e.g. user.Faction, user.Email, etc.
                // Example: check for a custom property
                if (user == null)
                {
                    // Fix: Redirect method expects a string and a boolean, not an anonymous type.
                    context.Response.Redirect("/Account/Login");
                    return;
                }

                if (user.TotalPlanets == 0)
                {
                    context.Response.Redirect("/Game/RestartEmpire");
                    return;
                }



                    // Get the ApplicationDbContext
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Query the ServerStats table (example: get the first row)
                    var serverStats = await dbContext.ServerStats.FirstOrDefaultAsync();

                if (serverStats.UWCompleted == true)
                {
                    if(userId == serverStats.UWHolderId)
                    {
                        context.Response.Redirect("/Game/UltimateWeaponCompleted");
                        return;
                    }
                    else
                    {
                        context.Response.Redirect("/Game/GalaxyEnd");
                        return;
                    }
                }
            }

            await next();
        });
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();
app.MapHub<ChatHub>("/chathub");
app.MapHub<TurnHub>("/turnhub");

app.Run();

