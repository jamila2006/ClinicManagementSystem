using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;

namespace ClinicManagementSystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly ICacheService _cacheService;
        public DepartmentService(IDepartmentRepository repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<List<DepartmentDto>> GetAllAsync(int pageNumber, int pageSize, string? sortBy)
        {
            var cacheKey = $"departments:all:page{pageNumber}:size{pageSize}:sort{sortBy}";

            var departments = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.GetAllAsync(pageNumber, pageSize, sortBy)
            );

            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description
            }).ToList();
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"department:{id}";

            var d = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.GetByIdAsync(id)
            );

            if (d == null) return null;

            return new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description
            };
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _repository.AddAsync(department);
            await _repository.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department == null) return false;

            department.Name = dto.Name;
            department.Description = dto.Description;
            _repository.Update(department);

            var success = await _repository.SaveChangesAsync();
            if (success) _cacheService.Remove($"department:{id}");
            return success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department == null) return false;
            _repository.Delete(department);

            var success = await _repository.SaveChangesAsync();
            if (success) _cacheService.Remove($"department:{id}");
            return success;
        }
    }
}