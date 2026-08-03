using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
        Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}