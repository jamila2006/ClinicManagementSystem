using ClinicManagementSystem.Constants;
using ClinicManagementSystem.Data;
using ClinicManagementSystem.DTOs;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    AppDbContext context,
    IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Yeni istifadəçi qeydiyyatı — ADMIN, DOCTOR və ya PATIENT rolu ilə.
        /// DOCTOR/PATIENT rolları üçün mövcud DoctorId/PatientId göstərilməlidir.
        /// </summary>
        /// <param name="dto">Qeydiyyat məlumatları (email, şifrə, rol, DoctorId/PatientId)</param>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var validRoles = new[] { Roles.Admin, Roles.Doctor, Roles.Patient };
            if (!validRoles.Contains(dto.Role))
            {
                return BadRequest(new { message = "Rol yalnız ADMIN, DOCTOR və ya PATIENT ola bilər." });
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Bu email artıq qeydiyyatdan keçib." });
            }

            if (dto.Role == Roles.Doctor)
            {
                if (dto.DoctorId == null)
                    return BadRequest(new { message = "DOCTOR rolu üçün DoctorId göstərilməlidir." });

                var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
                if (doctor == null)
                    return BadRequest(new { message = "Göstərilən DoctorId mövcud deyil." });

                var doctorTaken = await _context.Users.AnyAsync(u => u.DoctorId == dto.DoctorId);
                if (doctorTaken)
                    return BadRequest(new { message = "Bu Doctor qeydinə artıq bir hesab bağlıdır." });
            }

            if (dto.Role == Roles.Patient)
            {
                if (dto.PatientId == null)
                    return BadRequest(new { message = "PATIENT rolu üçün PatientId göstərilməlidir." });

                var patient = await _context.Patients.FindAsync(dto.PatientId);
                if (patient == null)
                    return BadRequest(new { message = "Göstərilən PatientId mövcud deyil." });

                var patientTaken = await _context.Users.AnyAsync(u => u.PatientId == dto.PatientId);
                if (patientTaken)
                    return BadRequest(new { message = "Bu Patient qeydinə artıq bir hesab bağlıdır." });
            }

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                DoctorId = dto.Role == Roles.Doctor ? dto.DoctorId : null,
                PatientId = dto.Role == Roles.Patient ? dto.PatientId : null
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new { message = "Qeydiyyat uğurludur." });
        
    }
        /// <summary>
        /// İstifadəçi girişi — email və şifrə ilə, uğurlu olduqda JWT token qaytarır.
        /// </summary>
        /// <param name="dto">Giriş məlumatları (email, şifrə)</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Email və ya şifrə yanlışdır." });
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                return Unauthorized(new { message = "Email və ya şifrə yanlışdır." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            var expiresInMinutes = double.Parse(_configuration["Jwt:ExpiresInMinutes"]!);

            return Ok(new AuthResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
            });
        }
    }
}