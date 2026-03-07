using Kuska.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---- Base de datos ----
builder.Services.AddDbContext<KuskaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KuskaDB")));

// ---- Servicios ----
builder.Services.AddScoped<AuditoriaService>();

// ---- MVC ----
builder.Services.AddControllersWithViews();

// ---- Sesión para autenticación simple ----
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();