using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;
        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<DoctorDTO>>> GetAll()
        {
            var doctors = await _service.GetAllAsync();
            return Ok(doctors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetById(int id)
        {
            var doctor = await _service.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }
        [HttpPost]
        public async Task<ActionResult<DoctorDTO>> Create(CreateDoctorDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateDoctorDTO dto)
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
