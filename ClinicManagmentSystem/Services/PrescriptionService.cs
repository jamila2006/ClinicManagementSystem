using ClinicManagementSystem.Data;
using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Exceptions;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;

namespace ClinicManagementSystem.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly INotificationQueue _notificationQueue;


        public PrescriptionService(IPrescriptionRepository repository, AppDbContext context, ICacheService cacheService, INotificationQueue notificationQueue)
        {
            _repository = repository;
            _context = context;
            _cacheService = cacheService;
            _notificationQueue = notificationQueue;
        }

        public async Task<PrescriptionDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"prescription:{id}";

            var prescription = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _repository.GetByIdWithDetailsAsync(id)
            );

            return prescription == null ? null : ToDto(prescription);
        }

        public async Task<(List<PrescriptionDto> Items, int TotalCount)> SearchAsync(PrescriptionFilterDto filter)
        {
            var (items, total) = await _repository.SearchAsync(filter);
            return (items.Select(ToDto).ToList(), total);
        }

        // Çox-cədvəlli tranzaksiya — Checkpoint 4
        public async Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var prescription = new Prescription
                {
                    AppointmentId = dto.AppointmentId,
                    DoctorId = dto.DoctorId,
                    PatientId = dto.PatientId,
                    Notes = dto.Notes ?? string.Empty,
                    IssueDate = DateTime.UtcNow
                };
                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync(); // Id lazımdır, item-lərə bağlamaq üçün

                foreach (var itemDto in dto.Items)
                {
                    var medication = await _repository.GetMedicationByIdAsync(itemDto.MedicationId)
                        ?? throw new KeyNotFoundException($"Medication (Id: {itemDto.MedicationId}) tapılmadı");

                    if (medication.StockQuantity < itemDto.Quantity)
                        throw new InsufficientStockException(
                            $"'{medication.Name}' üçün stok kifayət etmir (mövcud: {medication.StockQuantity}, tələb olunan: {itemDto.Quantity})");

                    medication.StockQuantity -= itemDto.Quantity;

                    _context.PrescriptionItems.Add(new PrescriptionItem
                    {
                        PrescriptionId = prescription.Id,
                        MedicationId = medication.Id,
                        Dosage = itemDto.Dosage,
                        Frequency = itemDto.Frequency,
                        Duration = itemDto.Duration,
                        Instructions = itemDto.Instructions,
                        Quantity = itemDto.Quantity
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var result = await _repository.GetByIdWithDetailsAsync(prescription.Id);
                _notificationQueue.Enqueue(new EmailNotification(
    result!.Patient?.Email ?? "unknown@clinic.com",
    "Yeni resept təyin edildi",
    $"Hörmətli {result.Patient?.FirstName}, sizə yeni resept təyin olundu."
));
                return ToDto(result!);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static PrescriptionDto ToDto(Prescription p) => new()
        {
            Id = p.Id,
            AppointmentId = p.AppointmentId,
            DoctorId = p.DoctorId,
            DoctorName = p.Doctor != null ? $"{p.Doctor.FirstName} {p.Doctor.LastName}" : string.Empty,
            PatientId = p.PatientId,
            PatientName = p.Patient != null ? $"{p.Patient.FirstName} {p.Patient.LastName}" : string.Empty,
            IssueDate = p.IssueDate,
            Notes = p.Notes,
            Items = p.Items.Select(i => new PrescriptionItemDto
            {
                Id = i.Id,
                MedicationId = i.MedicationId,
                MedicationName = i.Medication != null ? i.Medication.Name : string.Empty,
                Dosage = i.Dosage,
                Frequency = i.Frequency,
                Duration = i.Duration,
                Instructions = i.Instructions,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}