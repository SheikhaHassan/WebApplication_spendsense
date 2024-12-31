using Microsoft.EntityFrameworkCore;
using budgetManagement.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add session services
builder.Services.AddDistributedMemoryCache(); // Required for storing session data
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout duration
    options.Cookie.HttpOnly = true; // Ensures cookie is accessible only via HTTP
    options.Cookie.IsEssential = true; // Makes the session cookie essential for functionality
});

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()  // Allows any origin (for development purposes, use a specific URL in production)
               .AllowAnyMethod()  // Allows any HTTP method
               .AllowAnyHeader());  // Allows any headers
});

// Add DbContext (for Entity Framework)
builder.Services.AddDbContext<BudgetContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Web API controllers
builder.Services.AddControllers(); // Add services for Web API controllers

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use session middleware
app.UseSession();

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map MVC controllers and views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Web API controllers
app.MapControllers();  // This is for the Web API controllers to be available

app.Run();
