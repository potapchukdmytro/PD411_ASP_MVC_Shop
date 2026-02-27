using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Data.Initalizer;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.Services;
using PD411_Shop.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("LocalDb");
    options.UseSqlServer(connectionString);
});

// Add Identity
builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Add automapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg5NTE2ODAwIiwiaWF0IjoiMTc1ODAwNDY5MiIsImFjY291bnRfaWQiOiIwMTk5NTEzZTdlYmY3YjYwOGI4Y2I3NTI3YTE3ZTI5MyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazU4a3hoZXN2ZWI3aDZncms2MHBrYXJrIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.OMUeI0YxSQYUSUYehr5O6yevTWgsGamrSrCFSZ7Sd3fNsl01WU-pr6M6wusxNSxoQ6w8-lqrjOk6gj8KShQQhmvz91wRuRm_rObvAaDQEBRDit7iSUe6J7EH8lDmpqlUuJQ8zN0lCTgIDwaHDaI9h4FcSVy6qmi68oETGI876KCUf5ifCCwDSpZjirIws5XvO6IpQEkCp8FWd2UkTWvrHaaJWFbxOWfKbx_j5AeHPE1o5Piiz7qF6QKX8MzOj44f0yRExRKMCeQSauqRBgO33CooOm0mxbU2-Mx5tb3PPHdaFe7YxPKdRYSJ1TsRn3DELSrxnKsPE11X4eIXYuJh6w";
}, AppDomain.CurrentDomain.GetAssemblies());

// Add Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// DI налаштуванн€
// builder.Services.AddSingleton(); // патерн singleton - об'Їкт ≥снуЇ в Їдиному екземпл€р≥ на весь час роботи програми, створюЇтьс€ при першому запит≥ ≥ використовуЇтьс€ повторно дл€ вс≥х наступних запит≥в
// builder.Services.AddTransient(); // патерн transient - об'Їкт створюЇтьс€ кожного разу при запит≥, не збер≥гаЇтьс€ м≥ж запитами, використовуЇтьс€ дл€ легковагих серв≥с≥в, €к≥ не мають стану
// builder.Services.AddScoped(); // патерн scoped - об'Їкт створюЇтьс€ один раз на кожен HTTP-запит, збер≥гаЇтьс€ прот€гом всього запиту, використовуЇтьс€ дл€ серв≥с≥в, €к≥ мають стан, пов'€заний з конкретним запитом
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ImageService>();

// Add services
builder.Services.AddScoped<IEmailSender, EmailService>();

// Add options
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

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

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

Seeder.Seed(app);

app.Run();