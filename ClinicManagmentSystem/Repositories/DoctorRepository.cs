using ClinicManagementSystem.Data;
using ClinicManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
        }

        public async Task<List<Doctor>> GetAllAsync(int pageNumber, int pageSize, string? sortBy)
        {
            var query = _context.Doctors.Include(d => d.Department).AsQueryable();
            query = sortBy?.ToLower() switch
            {
                "firstname" => query.OrderBy(d => d.FirstName),
                "lastname" => query.OrderBy(d => d.LastName),
                "experienceyears" => query.OrderBy(d => d.ExperienceYears),
                _ => query.OrderBy(d => d.Id)
            };
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d=>d.Id== id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;// " >0 " ona gore yazdim ki int boola cevrilsin eks halda error verir 
        }

        public void Update(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
        }
    }
}
