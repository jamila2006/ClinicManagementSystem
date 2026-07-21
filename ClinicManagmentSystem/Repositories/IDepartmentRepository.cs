using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<Department?> GetByIdAsync(int id);
        Task AddAsync(Department department);
        void Update(Department department);
        void Delete(Department department);
        Task<bool> SaveChangesAsync();
    }
}