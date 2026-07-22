using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;
using ClinicManagementSystem.Services;
using Moq;

namespace ClinicManagementSystem.Tests
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repositoryMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repositoryMock = new Mock<IPatientRepository>();
            _service = new PatientService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedPatientDtos()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    Id = 1,
                    FirstName = "Nərmin",
                    LastName = "Əliyeva",
                    BirthDate = new DateTime(1995, 5, 20),
                    Gender = "Female",
                    Email = "narmin@example.com",
                    PhoneNumber = "0551234567"
                }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(1, 10, null))
                .ReturnsAsync(patients);

            // Act
            var result = await _service.GetAllAsync(1, 10, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Nərmin", result[0].FirstName);
            Assert.Equal("Əliyeva", result[0].LastName);
        }

        [Fact]
        public async Task GetByIdAsync_PatientExists_ReturnsPatientDto()
        {
            // Arrange
            var patient = new Patient
            {
                Id = 1,
                FirstName = "Nərmin",
                LastName = "Əliyeva",
                BirthDate = new DateTime(1995, 5, 20),
                Gender = "Female",
                Email = "narmin@example.com",
                PhoneNumber = "0551234567"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Nərmin", result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_PatientDoesNotExist_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_PatientExists_ReturnsTrue()
        {
            // Arrange
            var patient = new Patient { Id = 1, FirstName = "Nərmin", LastName = "Əliyeva" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _repositoryMock
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.Delete(patient), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_PatientDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}