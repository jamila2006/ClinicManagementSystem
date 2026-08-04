namespace ClinicManagementSystem.DTOs
{
    public class CreatePrescriptionItemDto
    {
        public int MedicationId { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
