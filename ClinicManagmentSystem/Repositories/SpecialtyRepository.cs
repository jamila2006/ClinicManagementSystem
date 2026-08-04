using ClinicManagementSystem.Data;
using ClinicManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly AppDbContext _context;
        public SpecialtyRepository(AppDbContext context) => _context = context;

        public async Task<List<Specialty>> GetAllAsync() =>
            await _context.Specialties.AsNoTracking().ToListAsync();

        public async Task<Specialty?> GetByIdAsync(int id) =>
            await _context.Specialties.FindAsync(id);

        public async Task<Specialty> AddAsync(Specialty specialty)
        {
            _context.Specialties.Add(specialty);
            await _context.SaveChangesAsync();
            return specialty;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null) return false;
            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}