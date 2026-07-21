using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentsController(IDepartmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Bütün şöbələrin siyahısını səhifələmə və sıralama ilə qaytarır.
        /// </summary>
        /// <param name="pageNumber">Səhifə nömrəsi (default: 1)</param>
        /// <param name="pageSize">Səhifədəki element sayı (default: 10)</param>
        /// <param name="sortBy">Sıralama sahəsi: name, name_desc</param>
        [HttpGet]
        [ProducesResponseType(typeof(List<DepartmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DepartmentDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var departments = await _service.GetAllAsync(pageNumber, pageSize, sortBy);
            return Ok(departments);
        }

        /// <summary>
        /// ID-yə görə bir şöbənin məlumatını qaytarır.
        /// </summary>
        /// <param name="id">Şöbənin ID-si</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentDto>> GetById(int id)
        {
            var department = await _service.GetByIdAsync(id);
            if (department == null) return NotFound();
            return Ok(department);
        }

        /// <summary>
        /// Yeni şöbə yaradır.
        /// </summary>
        /// <param name="dto">Yaradılacaq şöbənin məlumatları</param>
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Mövcud şöbənin məlumatlarını yeniləyir.
        /// </summary>
        /// <param name="id">Yenilənəcək şöbənin ID-si</param>
        /// <param name="dto">Yeni məlumatlar</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, CreateDepartmentDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Şöbəni sistemdən silir.
        /// </summary>
        /// <param name="id">Silinəcək şöbənin ID-si</param>
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