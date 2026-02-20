using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Models;
using System.Text.Json;

namespace PD411_Shop.Data.Initalizer
{
    public static class Seeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            // отримання AppDbContext із DI
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate(); // застосовує всі міграції як і команда "update-database"

            // Users and Roles
            if(!roleManager.Roles.Any())
            {
                var adminRole = new IdentityRole { Name = "admin" };
                var userRole = new IdentityRole { Name = "user" };

                roleManager.CreateAsync(adminRole).Wait();
                roleManager.CreateAsync(userRole).Wait();

                var admin = new UserModel
                {
                    Email = "admin@mail.com",
                    UserName = "admin",
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Doe"
                };

                var user = new UserModel
                {
                    Email = "user@mail.com",
                    UserName = "user",
                    EmailConfirmed = true,
                    FirstName = "Joe",
                    LastName = "Biden"
                };

                userManager.CreateAsync(admin, "qwerty").Wait();
                userManager.CreateAsync(user, "qwerty").Wait();

                userManager.AddToRoleAsync(admin, "admin").Wait();
                userManager.AddToRoleAsync(user, "user").Wait();
            }

            if (!context.Categories.Any())
            {
                string root = Directory.GetCurrentDirectory();
                string path = Path.Combine(root, "wwwroot", "seed_data", "components.json");
                string json = File.ReadAllText(path);
                List<CategoryModel>? categories = JsonSerializer.Deserialize<List<CategoryModel>>(json);

                if(categories == null)
                {
                    return;
                }

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}
