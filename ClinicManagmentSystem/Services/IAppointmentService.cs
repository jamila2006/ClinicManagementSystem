using ClinicManagementSystem.DTOs;

namespace ClinicManagementSystem.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task<bool> UpdateAsync(int id, CreateAppointmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}