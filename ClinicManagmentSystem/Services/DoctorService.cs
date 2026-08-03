using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;

namespace ClinicManagementSystem.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<DoctorDTO> CreateAsync(CreateDoctorDTO dto)
        {
            var doctor = new Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                ExperienceYears = dto.ExperienceYears,
                DepartmentId = dto.DepartmentId
            };
            await _repository.AddAsync(doctor);
            await _repository.SaveChangesAsync();
            return new DoctorDTO
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                ExperienceYears = doctor.ExperienceYears,
                DepartmentId = doctor.DepartmentId,

            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if(doctor == null) return false;
            _repository.Delete(doctor);
            return await _repository.SaveChangesAsync();
        }

        public async Task<List<DoctorDTO>> GetAllAsync(int pageNumber, int pageSize, string? sortBy)
        {
            var doctors = await _repository.GetAllAsync(pageNumber, pageSize, sortBy);
       
            return doctors.Select(d=> new DoctorDTO
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                ExperienceYears = d.ExperienceYears,
                DepartmentId = d.DepartmentId,
                DepartmentName=d.Department?.Name
            }).ToList();
        }

        public async Task<DoctorDTO?> GetByIdAsync(int id)
        {
            var d=await _repository.GetByIdAsync(id);
            if(d==null) return null;
            return new DoctorDTO
            {

                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                ExperienceYears = d.ExperienceYears,
                DepartmentId = d.DepartmentId,
                DepartmentName = d.Department?.Name
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor =await _repository.GetByIdAsync(id);
            if (doctor==null) return false;

            doctor.FirstName = dto.FirstName;
            doctor.LastName = dto.LastName;
            doctor.Email = dto.Email;
            doctor.PhoneNumber = dto.PhoneNumber;
            doctor.ExperienceYears = dto.ExperienceYears;
            doctor.DepartmentId = dto.DepartmentId;
            _repository.Update(doctor);
            return await _repository.SaveChangesAsync();
        }
    }
}
