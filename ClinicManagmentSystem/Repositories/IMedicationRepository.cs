using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repositories
{
    public interface IMedicationRepository
    {
        Task<List<Medication>> GetAllAsync();
        Task<Medication?> GetByIdAsync(int id);
        Task<Medication> AddAsync(Medication medication);
        Task<bool> UpdateAsync(int id, Medication updated);
        Task<bool> DeleteAsync(int id);
        Task<List<Medication>> GetLowStockAsync(int threshold);
    }
}
