using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<Prescription?> GetByIdWithDetailsAsync(int id);
        Task<List<Prescription>> GetByPatientAsync(int patientId, DateTime? from, DateTime? to);
        Task<List<Doctor>> GetDoctorsBySpecialtyAsync(string specialtyName);
        Task<(List<Prescription> Items, int TotalCount)> SearchAsync(PrescriptionFilterDto filter);
        Task<Prescription> AddAsync(Prescription prescription);
        Task<Medication?> GetMedicationByIdAsync(int medicationId);
    }
}
