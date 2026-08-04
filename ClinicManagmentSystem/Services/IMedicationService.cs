using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IMedicationService
    {
        Task<List<MedicationDto>> GetAllAsync();
        Task<MedicationDto?> GetByIdAsync(int id);
        Task<MedicationDto> CreateAsync(CreateMedicationDto dto);
        Task<bool> UpdateAsync(int id, UpdateMedicationDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<MedicationDto>> GetLowStockAsync(int threshold);
    }
}