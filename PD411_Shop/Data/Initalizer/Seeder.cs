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

            context.Database.Migrate(); // застосовує всі міграції як і команда "update-database"

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
