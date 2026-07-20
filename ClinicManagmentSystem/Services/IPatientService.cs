using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto> CreateAsync(CreatePatientDto dto);
        Task<bool> UpdateAsync(int id, CreatePatientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}