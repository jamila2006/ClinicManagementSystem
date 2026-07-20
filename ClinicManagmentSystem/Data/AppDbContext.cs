using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Appointment> Appointments{ get; set; }
        public DbSet<Department> Departments{ get; set; }
        public DbSet<Doctor> Doctors{ get; set; }
        public DbSet<Patient> Patients{ get; set; }
    }
}
