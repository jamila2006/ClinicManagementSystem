namespace ClinicManagementSystem.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
    }
}
