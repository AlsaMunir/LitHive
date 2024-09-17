using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication6.Data;
using Domain.Models;
using Infrastructure;
using WebApplication6.Hubs; 
using Domain.Interfaces;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
// Register generic repository
builder.Services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped(typeof(GenericService<>)); // Register generic service

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllers();
// IUserService and its implementation
builder.Services.AddScoped<IUserSevice, UserService>();

// database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>

{
   options.IdleTimeout = TimeSpan.FromMinutes(30); 
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
   
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<UserProfile>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BP", policy => policy.RequireAssertion(Context => DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 15));

    options.AddPolicy("PakOnly", policy => policy.RequireClaim(ClaimTypes.Country,"Pakistan"));

    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(context =>
        {
            var userEmail = context.User.FindFirstValue(ClaimTypes.Email);
            return userEmail == "alsamunir@gmail.com";
        });
    });

});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.MapRazorPages();
app.Run();
