using ClinicManagementSystem.Data;
using ClinicManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllAsync(int pageNumber, int pageSize, string? sortBy)
        {
            var query = _context.Patients.AsQueryable();

            query = sortBy?.ToLower() switch
            {
                "firstname" => query.OrderBy(p => p.FirstName),
                "lastname" => query.OrderBy(p => p.LastName),
                _ => query.OrderBy(p => p.Id)
            };

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }

        public void Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}