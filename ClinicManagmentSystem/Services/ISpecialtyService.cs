using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface ISpecialtyService
    {
        Task<List<SpecialtyDto>> GetAllAsync();
        Task<SpecialtyDto?> GetByIdAsync(int id);
        Task<SpecialtyDto> CreateAsync(CreateSpecialtyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
