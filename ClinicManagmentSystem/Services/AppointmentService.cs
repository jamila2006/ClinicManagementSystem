using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;

namespace ClinicManagementSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly ICacheService _cacheService;
        private static readonly TimeSpan AppointmentCacheExpiration = TimeSpan.FromMinutes(1);

        public AppointmentService(IAppointmentRepository repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<List<AppointmentDto>> GetAllAsync(int pageNumber, int pageSize, string? sortBy)
        {
            var cacheKey = $"appointments:all:page{pageNumber}:size{pageSize}:sort{sortBy}";

            var appointments = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.GetAllAsync(pageNumber, pageSize, sortBy),
                AppointmentCacheExpiration
            );

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                Notes = a.Notes,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor != null ? $"{a.Doctor.FirstName} {a.Doctor.LastName}" : null,
                PatientId = a.PatientId,
                PatientName = a.Patient != null ? $"{a.Patient.FirstName} {a.Patient.LastName}" : null
            }).ToList();
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"appointment:{id}";

            var a = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.GetByIdAsync(id),
                AppointmentCacheExpiration
            );

            if (a == null) return null;

            return new AppointmentDto
            {
                Id = a.Id,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                Notes = a.Notes,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor != null ? $"{a.Doctor.FirstName} {a.Doctor.LastName}" : null,
                PatientId = a.PatientId,
                PatientName = a.Patient != null ? $"{a.Patient.FirstName} {a.Patient.LastName}" : null
            };
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var appointment = new Appointment
            {
                AppointmentDate = dto.AppointmentDate,
                Status = dto.Status,
                Notes = dto.Notes,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId
            };

            await _repository.AddAsync(appointment);
            await _repository.SaveChangesAsync();

            return new AppointmentDto
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null) return false;

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.Status = dto.Status;
            appointment.Notes = dto.Notes;
            appointment.DoctorId = dto.DoctorId;
            appointment.PatientId = dto.PatientId;

            _repository.Update(appointment);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null) return false;

            _repository.Delete(appointment);
            return await _repository.SaveChangesAsync();
        }
    }
}