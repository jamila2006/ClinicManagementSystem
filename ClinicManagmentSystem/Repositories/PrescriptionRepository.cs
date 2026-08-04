using ClinicManagementSystem.Data;
using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;
        public PrescriptionRepository(AppDbContext context) => _context = context;

        // N+1 problemi HƏLL OLUNMUŞ versiya — Checkpoint 5
        public async Task<Prescription?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Items).ThenInclude(i => i.Medication)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // LINQ / derived query analoqu — Checkpoint 2
        public async Task<List<Prescription>> GetByPatientAsync(int patientId, DateTime? from, DateTime? to)
        {
            var query = _context.Prescriptions.Where(p => p.PatientId == patientId);

            if (from.HasValue) query = query.Where(p => p.IssueDate >= from.Value);
            if (to.HasValue) query = query.Where(p => p.IssueDate <= to.Value);

            return await query
                .Include(p => p.Items).ThenInclude(i => i.Medication)
                .Include(p => p.Doctor)
                .OrderByDescending(p => p.IssueDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetDoctorsBySpecialtyAsync(string specialtyName)
        {
            return await _context.Doctors
                .Where(d => d.Specialties.Any(s => s.Name == specialtyName))
                .AsNoTracking()
                .ToListAsync();
        }

        // Dinamik axtarış/filtrasiya — Checkpoint 3
        public async Task<(List<Prescription> Items, int TotalCount)> SearchAsync(PrescriptionFilterDto filter)
        {
            IQueryable<Prescription> query = _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Items).ThenInclude(i => i.Medication);

            if (filter.DoctorId.HasValue)
                query = query.Where(p => p.DoctorId == filter.DoctorId.Value);

            if (filter.PatientId.HasValue)
                query = query.Where(p => p.PatientId == filter.PatientId.Value);

            if (!string.IsNullOrWhiteSpace(filter.MedicationName))
                query = query.Where(p => p.Items.Any(i => i.Medication.Name.Contains(filter.MedicationName)));

            if (filter.DateFrom.HasValue)
                query = query.Where(p => p.IssueDate >= filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
                query = query.Where(p => p.IssueDate <= filter.DateTo.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.IssueDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, total);
        }

        public async Task<Prescription> AddAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task<Medication?> GetMedicationByIdAsync(int medicationId) =>
            await _context.Medications.FindAsync(medicationId);
    }
}