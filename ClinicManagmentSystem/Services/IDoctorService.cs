using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDTO>> GetAllAsync();
        Task<DoctorDTO?> GetByIdAsync(int id);
        Task<DoctorDTO> CreateAsync(CreateDoctorDTO dto);
        Task<bool> UpdateAsync(int id, CreateDoctorDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
