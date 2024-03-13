using Book_App.Data;
using Book_App.Models;
using Book_App.Services.BookServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddDefaultIdentity<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>() // Add roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IBookService, BookService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            // Check if there are already genres in the database
            if (!context.Genres.Any())
            {
                // Add some example genres
                var genres = new List<Genre>
                {
                    new Genre { Name = "Fiction" },
                    new Genre { Name = "Mystery" },
                    new Genre { Name = "Thriller" },
                    new Genre { Name = "Romantic" },
                    new Genre { Name = "Classics" },
                    new Genre { Name = "Sci-Fi" },
                    new Genre { Name = "Horror" },
                };

                context.Genres.AddRange(genres);
                context.SaveChanges();
            }

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            var roles = new string[] { "Administrator" };

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    // Create the role
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Check if there are already users with Administrator role
            var adminUser = await userManager.FindByNameAsync("admin@example.com");

            if (adminUser == null)
            {
                // If no user found, create an admin user
                var admin = new User
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                };

                var result = await userManager.CreateAsync(admin, "AdminPassword123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Administrator");
                }
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
