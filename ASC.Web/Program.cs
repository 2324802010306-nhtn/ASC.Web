using ASC.DataAccess;
using ASC.DataAccess.Interfaces;
using ASC.Web; // 👈 dùng ApplicationSettings
using ASC.Web.Data;
using ASC.Web.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// ================= DATABASE =================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 👉 FIX ambiguous DbContext bằng FULL NAME
builder.Services.AddDbContext<ASC.Web.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ================= IDENTITY =================
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
// 👉 FIX ambiguous DbContext
.AddEntityFrameworkStores<ASC.Web.Data.ApplicationDbContext>()
.AddDefaultTokenProviders();

// ================= CONFIG =================
builder.Services.Configure<ApplicationSettings>(
    builder.Configuration.GetSection("AppSettings"));

// ================= MVC =================
builder.Services.AddControllersWithViews();

// ================= SERVICES =================
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
builder.Services.AddTransient<ISmsSender, AuthMessageSender>();

// ================= CUSTOM SERVICES =================
builder.Services.AddSingleton<IIdentitySeed, IdentitySeed>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build(); // ✅ không còn lỗi

// ================= PIPELINE =================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

// ================= ROUTING =================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// ================= SEED =================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var seed = services.GetRequiredService<IIdentitySeed>();

    await seed.Seed(
        services.GetRequiredService<UserManager<IdentityUser>>(),
        services.GetRequiredService<RoleManager<IdentityRole>>(),
        services.GetRequiredService<IOptions<ApplicationSettings>>() // 👈 đúng type
    );
}

app.Run();