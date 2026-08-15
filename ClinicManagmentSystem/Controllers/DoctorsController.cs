using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Repositories;
using ClinicManagementSystem.Services;
using ClinicManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    [Authorize(Roles = "ADMIN,DOCTOR")]
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;
        private readonly IFileService _fileService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICacheService _cacheService;   // <- YENİ

        public DoctorsController(IDoctorService service, IFileService fileService, IDoctorRepository doctorRepository, ICacheService cacheService)
        {
            _service = service;
            _fileService = fileService;
            _doctorRepository = doctorRepository;
            _cacheService = cacheService;   // <- YENİ
        }

        // ... GetAll, GetById, Create, Update, Delete — DƏYİŞMƏZ QALIR ...

        [HttpPost("{id}/photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadPhoto(int id, IFormFile file)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();

            try
            {
                var savedFileName = await _fileService.SaveDoctorPhotoAsync(id, file);

                _fileService.DeleteDoctorPhoto(doctor.PhotoUrl);   // <- DÜZƏLDİ (Async yox)

                doctor.PhotoUrl = savedFileName;
                _doctorRepository.Update(doctor);
                await _doctorRepository.SaveChangesAsync();

                _cacheService.Remove($"doctor:{id}");   // <- YENİ

                return Ok(new { photoUrl = savedFileName });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GetPhoto — dəyişməz qalır
    }
}