using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDTO>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<DoctorDTO?> GetByIdAsync(int id);
        Task<DoctorDTO> CreateAsync(CreateDoctorDTO dto);
        Task<bool> UpdateAsync(int id, UpdateDoctorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
