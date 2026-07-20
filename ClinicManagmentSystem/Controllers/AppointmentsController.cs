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

        [HttpGet]
        public async Task<ActionResult<List<AppointmentDto>>> GetAll()
        {
            var appointments = await _service.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            var appointment = await _service.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateAppointmentDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}