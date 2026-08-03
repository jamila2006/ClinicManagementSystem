using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto> CreateAsync(CreatePatientDto dto);
        Task<bool> UpdateAsync(int id, UpdatePatientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}