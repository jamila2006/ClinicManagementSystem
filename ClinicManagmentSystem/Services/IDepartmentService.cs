using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
        Task<bool> UpdateAsync(int id, CreateDepartmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}