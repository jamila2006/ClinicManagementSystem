using ClinicManagementSystem.Data;
using ClinicManagementSystem.Exceptions;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;
using ClinicManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;


namespace ClinicManagementSystem.Tests
{
    public class PrescriptionServiceRollbackTests
    {
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CreatePrescription_InsufficientStock_RollsBackEverything()
        {
            // Arrange
            await using var context = CreateContext();

            // Seed data
            var medication = new Medication
            {
                Name = "Amoxicillin",
                StockQuantity = 5,
                Price = 2.5m,
                Manufacturer = "Test",
                Form = "Tablet",
                Strength = 500,
                Description = "Test med"
            };
            context.Medications.Add(medication);
            await context.SaveChangesAsync();

            var repository = new PrescriptionRepository(context);
            var service = new PrescriptionService(repository, context);

            var dto = new ClinicManagementSystem.DTOs.CreatePrescriptionDto
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientId = 1,
                Items = new()
                {
                    new()
                    {
                        MedicationId = medication.Id,
                        Quantity = 10, // Stok 5-dən çoxdur → xəta!
                        Dosage = "500mg",
                        Frequency = "2x",
                        Duration = "5 days",
                        Instructions = "Take with food"
                    }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InsufficientStockException>(() => service.CreateAsync(dto));

            // Verify rollback
            var prescriptions = await context.Prescriptions.ToListAsync();
            var prescriptionItems = await context.PrescriptionItems.ToListAsync();
            var reloadedMedication = await context.Medications.FirstAsync();

            Assert.Empty(prescriptions); // Heç bir Prescription qalmamalıdır
            Assert.Empty(prescriptionItems); // Heç bir PrescriptionItem qalmamalıdır
            Assert.Equal(5, reloadedMedication.StockQuantity); // Stok dəyişməməlidir
        }
    }
}