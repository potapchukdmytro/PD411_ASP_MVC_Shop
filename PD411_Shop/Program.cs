using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Data.Initalizer;
using PD411_Shop.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("LocalDb");
    options.UseSqlServer(connectionString);
});

// DI наш≥ класи
// builder.Services.AddSingleton(); // патерн Singleton - об'Їкт класу буде ≥снувати в Їдиному екзепл€р≥
// builder.Services.AddTransient(); // об'Їкт класу буде створюватис€ при кожному використанн≥
builder.Services.AddScoped<ProductRepostitory>(); // об'Їкт класу буде створюватис€ при запит≥ та видал€ти п≥сл€ його завершенн€

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

Seeder.Seed(app);

app.Run();