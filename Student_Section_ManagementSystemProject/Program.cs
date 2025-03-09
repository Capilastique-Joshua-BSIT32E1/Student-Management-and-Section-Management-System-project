using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("StudentDB"));

var app = builder.Build();

//// Seed test data before running the app
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//    // Seed Subjects if empty
//    if (!context.Subjects.Any())
//    {
//        context.Subjects.AddRange(
//            new Subject { Name = "Mathematics", Description = "Basic Math" },
//            new Subject { Name = "Science", Description = "Physics & Chemistry" }
//        );
//        context.SaveChanges();
//    }

//    // Seed Students if empty
//    if (!context.Students.Any())
//    {
//        context.Students.AddRange(
//            new Student { Name = "John Doe", Email = "john@example.com" },
//            new Student { Name = "Jane Smith", Email = "jane@example.com" }
//        );
//        context.SaveChanges();
//    }
//}

// Middleware configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}"
);

app.Run();
