using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Services;
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
        public DoctorsController(IDoctorService service)
        {
            _service = service;
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
    }
}