using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Repositories
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAsync(int pageNumber, int pageSize, string? sortBy);
        Task<Appointment?> GetByIdAsync(int id);
        Task AddAsync(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Appointment appointment);
        Task<bool> SaveChangesAsync();
    }
}