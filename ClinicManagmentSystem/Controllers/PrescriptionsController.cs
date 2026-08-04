using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Exceptions;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    [Authorize(Roles = "ADMIN,DOCTOR")]
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        public PrescriptionsController(IPrescriptionService service) => _service = service;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prescription = await _service.GetByIdAsync(id);
            if (prescription == null) return NotFound();
            return Ok(prescription);
        }

        // Dinamik axtarış/filtrasiya — Checkpoint 3
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] PrescriptionFilterDto filter)
        {
            var (items, total) = await _service.SearchAsync(filter);
            return Ok(new { total, page = filter.PageNumber, pageSize = filter.PageSize, items });
        }

        // Çox-cədvəlli tranzaksiya — Checkpoint 4
        [HttpPost]
        public async Task<IActionResult> Create(CreatePrescriptionDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InsufficientStockException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}