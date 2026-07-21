using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Bütün görüşlərin siyahısını səhifələmə və sıralama ilə qaytarır.
        /// </summary>
        /// <param name="pageNumber">Səhifə nömrəsi (default: 1)</param>
        /// <param name="pageSize">Səhifədəki element sayı (default: 10)</param>
        /// <param name="sortBy">Sıralama sahəsi: date, date_desc, status</param>
        [HttpGet]
        [ProducesResponseType(typeof(List<AppointmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var appointments = await _service.GetAllAsync(pageNumber, pageSize, sortBy);
            return Ok(appointments);
        }

        /// <summary>
        /// ID-yə görə bir görüşün məlumatını qaytarır.
        /// </summary>
        /// <param name="id">Görüşün ID-si</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            var appointment = await _service.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        /// <summary>
        /// Yeni görüş yaradır.
        /// </summary>
        /// <param name="dto">Yaradılacaq görüşün məlumatları</param>
        [HttpPost]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Mövcud görüşün məlumatlarını yeniləyir.
        /// </summary>
        /// <param name="id">Yenilənəcək görüşün ID-si</param>
        /// <param name="dto">Yeni məlumatlar</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, CreateAppointmentDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Görüşü sistemdən silir.
        /// </summary>
        /// <param name="id">Silinəcək görüşün ID-si</param>
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