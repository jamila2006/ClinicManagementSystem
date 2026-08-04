namespace ClinicManagementSystem.DTOs
{
    public class CreatePrescriptionDto
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string? Notes { get; set; }
        public List<CreatePrescriptionItemDto> Items { get; set; } = new();
    }
}
