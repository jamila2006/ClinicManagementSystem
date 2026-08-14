using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;

namespace ClinicManagementSystem.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repository;
        private readonly ICacheService _cacheService;

        public MedicationService(IMedicationRepository repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<List<MedicationDto>> GetAllAsync()
        {
            var meds = await _cacheService.GetOrCreateAsync(
                "medications:all",
                async () => await _repository.GetAllAsync()
            );
            return meds.Select(ToDto).ToList();
        }

        public async Task<MedicationDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"medication:{id}";

            var med = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.GetByIdAsync(id)
            );

            return med == null ? null : ToDto(med);
        }

        public async Task<MedicationDto> CreateAsync(CreateMedicationDto dto)
        {
            var medication = new Medication
            {
                Name = dto.Name,
                Manufacturer = dto.Manufacturer,
                Strength = dto.Strength,
                Form = dto.Form,
                Description = dto.Description,
                StockQuantity = dto.StockQuantity,
                Price = dto.Price
            };
            var created = await _repository.AddAsync(medication);
            return ToDto(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdateMedicationDto dto)
        {
            var medication = new Medication
            {
                Name = dto.Name,
                Manufacturer = dto.Manufacturer,
                Strength = dto.Strength,
                Form = dto.Form,
                Description = dto.Description,
                StockQuantity = dto.StockQuantity,
                Price = dto.Price
            };
            var success = await _repository.UpdateAsync(id, medication);
            if (success) _cacheService.Remove($"medication:{id}");
            return success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (success) _cacheService.Remove($"medication:{id}");
            return success;
        }

        public async Task<List<MedicationDto>> GetLowStockAsync(int threshold)
        {
            var meds = await _repository.GetLowStockAsync(threshold);
            return meds.Select(ToDto).ToList();
        }

        private static MedicationDto ToDto(Medication m) => new()
        {
            Id = m.Id,
            Name = m.Name,
            Manufacturer = m.Manufacturer,
            Strength = m.Strength,
            Form = m.Form,
            Description = m.Description,
            StockQuantity = m.StockQuantity,
            Price = m.Price
        };
    }
}