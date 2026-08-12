using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;
using ClinicManagementSystem.Services.Implementations;

namespace ClinicManagementSystem.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly ICacheService _cacheService;   // <- BU SƏTIR ƏLAVƏ OLUNMALIDIR

        public PatientService(IPatientRepository repository, ICacheService cacheService)  // <- İKİNCİ PARAMETR ƏLAVƏ OLUNMALIDIR
        {
            _repository = repository;
            _cacheService = cacheService;   // <- BU SƏTIR ƏLAVƏ OLUNMALIDIR
        }

        public async Task<List<PatientDto>> GetAllAsync(int pageNumber, int pageSize, string? sortBy)
        {
            var cacheKey = $"patients:all:page{pageNumber}:size{pageSize}:sort{sortBy}";

            var patients = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.GetAllAsync(pageNumber, pageSize, sortBy)
            );

            return patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthDate = p.BirthDate,
                Gender = p.Gender,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber
            }).ToList();
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"patient:{id}";

            var p = await _cacheService.GetOrCreateAsync(   
                cacheKey,
                async () => await _repository.GetByIdAsync(id)  
            );

            if (p == null) return null;

            return new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthDate = p.BirthDate,
                Gender = p.Gender,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber
            };
        }

        public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            await _repository.AddAsync(patient);
            await _repository.SaveChangesAsync();

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.BirthDate = dto.BirthDate;
            patient.Gender = dto.Gender;
            patient.Email = dto.Email;
            patient.PhoneNumber = dto.PhoneNumber;

            _repository.Update(patient);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            _repository.Delete(patient);
            return await _repository.SaveChangesAsync();
        }
    }
}