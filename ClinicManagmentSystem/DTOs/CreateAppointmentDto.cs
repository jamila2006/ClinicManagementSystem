namespace ClinicManagementSystem.DTOs
{
    public class CreateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}