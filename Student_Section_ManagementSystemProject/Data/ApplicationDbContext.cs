using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Models;

namespace Student_Section_ManagementSystemProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
