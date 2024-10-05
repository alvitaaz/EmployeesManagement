using Microsoft.EntityFrameworkCore;
using EmployeesManagement.Models;

namespace EmployeesManagement.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet untuk entitas Employee
        public DbSet<Employee> Employees { get; set; }

        // DbSet untuk entitas ActivityLog
        public DbSet<ActivityLog> ActivityLogs { get; set; }  // menggunakan ActivityLogs as nama

        // Konfigurasi tambahan untuk model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurasi Primary Key untuk ActivityLog
            modelBuilder.Entity<ActivityLog>()
                .HasKey(a => a.LogId); 
        }
    }
}
