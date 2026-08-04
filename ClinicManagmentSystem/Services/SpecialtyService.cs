using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;

namespace ClinicManagementSystem.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ISpecialtyRepository _repository;
        public SpecialtyService(ISpecialtyRepository repository) => _repository = repository;

        public async Task<List<SpecialtyDto>> GetAllAsync()
        {
            var specialties = await _repository.GetAllAsync();
            return specialties.Select(ToDto).ToList();
        }

        public async Task<SpecialtyDto?> GetByIdAsync(int id)
        {
            var specialty = await _repository.GetByIdAsync(id);
            return specialty == null ? null : ToDto(specialty);
        }

        public async Task<SpecialtyDto> CreateAsync(CreateSpecialtyDto dto)
        {
            var specialty = new Specialty { Name = dto.Name };
            var created = await _repository.AddAsync(specialty);
            return ToDto(created);
        }

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

        private static SpecialtyDto ToDto(Specialty s) => new() { Id = s.Id, Name = s.Name };
    }
}