namespace ClinicManagementSystem.DTOs
{
    public class PrescriptionFilterDto
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public string? MedicationName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
