using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<List<Specialty>> GetAllAsync();
        Task<Specialty?> GetByIdAsync(int id);
        Task<Specialty> AddAsync(Specialty specialty);
        Task<bool> DeleteAsync(int id);
    }
}
