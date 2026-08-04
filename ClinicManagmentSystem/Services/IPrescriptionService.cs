using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IPrescriptionService
    {
        Task<PrescriptionDto?> GetByIdAsync(int id);
        Task<(List<PrescriptionDto> Items, int TotalCount)> SearchAsync(PrescriptionFilterDto filter);
        Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto dto);
    }
}