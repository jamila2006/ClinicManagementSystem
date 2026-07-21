using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repositories
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<Doctor?> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(Doctor doctor);
        Task<bool> SaveChangesAsync();
    }
}
