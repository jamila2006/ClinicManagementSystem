using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task<bool> UpdateAsync(int id, CreateAppointmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}