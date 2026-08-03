using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    [Authorize(Roles = "ADMIN,DOCTOR,PATIENT")]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        /// <summary>
        /// Bütün xəstələrin siyahısını səhifələmə və sıralama ilə qaytarır.
        /// </summary>
        /// <param name="pageNumber">Səhifə nömrəsi (default: 1)</param>
        /// <param name="pageSize">Səhifədəki element sayı (default: 10)</param>
        /// <param name="sortBy">Sıralama sahəsi: firstname, lastname</param>
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var patients = await _service.GetAllAsync(pageNumber, pageSize, sortBy);
            return Ok(patients);
        }

        /// <summary>
        /// ID-yə görə bir xəstənin məlumatını qaytarır.
        /// </summary>
        /// <param name="id">Xəstənin ID-si</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> GetById(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        /// <summary>
        /// Yeni xəstə yaradır.
        /// </summary>
        /// <param name="dto">Yaradılacaq xəstənin məlumatları</param>
        [HttpPost]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PatientDto>> Create(CreatePatientDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Mövcud xəstənin məlumatlarını yeniləyir.
        /// </summary>
        /// <param name="id">Yenilənəcək xəstənin ID-si</param>
        /// <param name="dto">Yeni məlumatlar</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdatePatientDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Xəstəni sistemdən silir.
        /// </summary>
        /// <param name="id">Silinəcək xəstənin ID-si</param>
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