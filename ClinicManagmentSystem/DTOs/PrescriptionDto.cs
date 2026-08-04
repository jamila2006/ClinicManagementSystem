namespace ClinicManagementSystem.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();
    }
}
