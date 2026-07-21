using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
        Task<bool> SaveChangesAsync();
    }
}
