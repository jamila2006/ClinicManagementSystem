using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetAll()
        {
            var patients = await _service.GetAllAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetById(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create(CreatePatientDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreatePatientDto dto)
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