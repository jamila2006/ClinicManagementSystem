using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.DTOs
{
    public class UpdateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        [Required]
        public string Status { get; set; }
        public string? Notes { get; set; }
        [Range(1, 999999, ErrorMessage = "Valid Doctor is required")]
        public int DoctorId { get; set; }
        [Range(1, 999999, ErrorMessage = "Valid Patient is required")]
        public int PatientId { get; set; }
    }
}