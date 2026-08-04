using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;

namespace ClinicManagementSystem.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repository;
        public MedicationService(IMedicationRepository repository) => _repository = repository;

        public async Task<List<MedicationDto>> GetAllAsync()
        {
            var meds = await _repository.GetAllAsync();
            return meds.Select(ToDto).ToList();
        }

        public async Task<MedicationDto?> GetByIdAsync(int id)
        {
            var med = await _repository.GetByIdAsync(id);
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

        public Task<bool> UpdateAsync(int id, UpdateMedicationDto dto)
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
            return _repository.UpdateAsync(id, medication);
        }

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

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