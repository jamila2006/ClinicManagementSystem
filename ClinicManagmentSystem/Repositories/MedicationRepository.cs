using ClinicManagementSystem.Data;
using ClinicManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Repositories
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly AppDbContext _context;
        public MedicationRepository(AppDbContext context) => _context = context;

        public async Task<List<Medication>> GetAllAsync() =>
            await _context.Medications.AsNoTracking().ToListAsync();

        public async Task<Medication?> GetByIdAsync(int id) =>
            await _context.Medications.FindAsync(id);

        public async Task<Medication> AddAsync(Medication medication)
        {
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();
            return medication;
        }

        public async Task<bool> UpdateAsync(int id, Medication updated)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null) return false;

            medication.Name = updated.Name;
            medication.Manufacturer = updated.Manufacturer;
            medication.Strength = updated.Strength;
            medication.Form = updated.Form;
            medication.Description = updated.Description;
            medication.StockQuantity = updated.StockQuantity;
            medication.Price = updated.Price;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null) return false;
            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();
            return true;
        }

        // Native (raw SQL) sorğu nümunəsi — Checkpoint 2
        public async Task<List<Medication>> GetLowStockAsync(int threshold)
        {
            return await _context.Medications
                .FromSqlInterpolated($"SELECT * FROM Medications WHERE StockQuantity < {threshold}")
                .ToListAsync();
        }
    }
}