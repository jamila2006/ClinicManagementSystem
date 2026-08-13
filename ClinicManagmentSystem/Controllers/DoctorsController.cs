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
        public DoctorsController(IDoctorService service, IFileService fileService,IDoctorRepository doctorRepository)
        {
            _service = service;
            _fileService = fileService;
            _doctorRepository = doctorRepository;
        }

        /// <summary>
        /// Bütün həkimlərin siyahısını səhifələmə və sıralama ilə qaytarır.
        /// </summary>
        /// <param name="pageNumber">Səhifə nömrəsi (default: 1)</param>
        /// <param name="pageSize">Səhifədəki element sayı (default: 10)</param>
        /// <param name="sortBy">Sıralama sahəsi: firstname, lastname, experienceyears</param>
        /// <returns>Həkimlərin siyahısı</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DoctorDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var doctors = await _service.GetAllAsync(pageNumber, pageSize, sortBy);
            return Ok(doctors);
        }

        /// <summary>
        /// ID-yə görə bir həkimin məlumatını qaytarır.
        /// </summary>
        /// <param name="id">Həkimin ID-si</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DoctorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DoctorDTO>> GetById(int id)
        {
            var doctor = await _service.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        /// <summary>
        /// Yeni həkim yaradır.
        /// </summary>
        /// <param name="dto">Yaradılacaq həkimin məlumatları</param>
        [HttpPost]
        [ProducesResponseType(typeof(DoctorDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DoctorDTO>> Create(CreateDoctorDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Mövcud həkimin məlumatlarını yeniləyir.
        /// </summary>
        /// <param name="id">Yenilənəcək həkimin ID-si</param>
        /// <param name="dto">Yeni məlumatlar</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateDoctorDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Həkimi sistemdən silir.
        /// </summary>
        /// <param name="id">Silinəcək həkimin ID-si</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        /// <summary>
        /// Həkimin profil şəklini yükləyir (yalnız .jpg/.jpeg/.png, maksimum 2MB).
        /// </summary>
        /// <param name="id">Həkimin ID-si</param>
        /// <param name="file">Yüklənəcək şəkil faylı</param>
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

                // köhnə foto varsa sil
                _fileService.DeleteDoctorPhotoAsync(doctor.PhotoUrl);

                doctor.PhotoUrl = savedFileName;
                _doctorRepository.Update(doctor);
                await _doctorRepository.SaveChangesAsync();

                return Ok(new { photoUrl = savedFileName });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Həkimin profil şəklini endirir.
        /// </summary>
        /// <param name="id">Həkimin ID-si</param>
        [HttpGet("{id}/photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null || string.IsNullOrEmpty(doctor.PhotoUrl))
                return NotFound();

            var result = await _fileService.GetDoctorPhotoAsync(doctor.PhotoUrl);
            if (result == null) return NotFound();

            return File(result.Value.Content, result.Value.ContentType, result.Value.FileName);
        }
    }
}