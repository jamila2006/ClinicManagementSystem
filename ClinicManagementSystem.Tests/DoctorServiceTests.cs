using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Repositories;
using ClinicManagementSystem.Services;
using Moq;

namespace ClinicManagementSystem.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repositoryMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _repositoryMock = new Mock<IDoctorRepository>();
            _service = new DoctorService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDoctorDtos()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    Id = 1,
                    FirstName = "Aygün",
                    LastName = "Məmmədova",
                    Email = "aygun@example.com",
                    PhoneNumber = "0501234567",
                    ExperienceYears = 5,
                    DepartmentId = 1
                }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(1, 10, null))
                .ReturnsAsync(doctors);

            // Act
            var result = await _service.GetAllAsync(1, 10, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Aygün", result[0].FirstName);
            Assert.Equal("Məmmədova", result[0].LastName);
        }

        [Fact]
        public async Task GetByIdAsync_DoctorExists_ReturnsDoctorDto()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = 1,
                FirstName = "Aygün",
                LastName = "Məmmədova",
                Email = "aygun@example.com",
                PhoneNumber = "0501234567",
                ExperienceYears = 5,
                DepartmentId = 1
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Aygün", result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_DoctorDoesNotExist_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Doctor?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_DoctorExists_ReturnsTrue()
        {
            // Arrange
            var doctor = new Doctor { Id = 1, FirstName = "Aygün", LastName = "Məmmədova" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _repositoryMock
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.Delete(doctor), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DoctorDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Doctor?)null);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}